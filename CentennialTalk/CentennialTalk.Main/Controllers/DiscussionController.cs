using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/chat")]
    public class DiscussionController : BaseController
    {
        private readonly IChatService chatService;
        private readonly IUnitOfWorkService uowService;
        private readonly IFileService fileService;

        public DiscussionController(IChatService chatService, IFileService fileService,
            IUnitOfWorkService uowService)
        {
            this.chatService = chatService;
            this.uowService = uowService;
            this.fileService = fileService;
        }

        [HttpPost("new")]
        public IActionResult New([FromBody]NewChatDTO newChat)
        {
            if (string.IsNullOrWhiteSpace(newChat.title))
                return GetJson(new ResponseDTO(ResponseCode.ERROR, "Unable to create chat with incomplete data"));

            Discussion chat = chatService.CreateNewChat(newChat);

            uowService.SaveChanges();

            if (chat != null)
            {
                DiscussionDTO dto = chat.GetResponseDTO();
                dto.username = dto.moderator;
                return GetJson(new ResponseDTO(ResponseCode.OK, dto));
            }
            else
                return GetJson(new ResponseDTO(ResponseCode.ERROR, "Error creating chat"));
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody]JoinChatDTO joinChat)
        {
            ResponseDTO result = chatService.JoinChat(joinChat);

            bool saved = uowService.SaveChanges();

            if (!saved)
                return GetJson(new ResponseDTO(ResponseCode.ERROR,
                    "Error while saving message"));

            return GetJson(result);
        }

        [HttpPost("close")]
        public IActionResult CloseJoiningLink([FromBody]RequestDTO chatCode)
        {
            Discussion chat = chatService.GetChatByCode(chatCode.value.ToString());

            if (chat == null)
                return GetJson(new ResponseDTO(ResponseCode.ERROR,
                    "No Chat with such code exists"));

            if (!chat.IsLinkOpen)
                return GetJson(new ResponseDTO(ResponseCode.MESSAGE, "Chat is already closed"));

            uowService.SaveChanges();

            return GetJson(new ResponseDTO(ResponseCode.OK, "Success"));
        }

        [HttpPost("transcript")]
        public async Task<IActionResult> Download(string chatCode)
        {
            string doc = fileService.CreateWordDocument(chatCode);

            MemoryStream memory = new MemoryStream();
            using (FileStream stream = new FileStream(doc, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.ms-word", doc);
        }
    }
}