using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        //[Authorize]
        [HttpPost("new")]
        public IActionResult New([FromBody]NewChatDTO newChat)
        {
            if (string.IsNullOrWhiteSpace(newChat.title))
                return GetJson(new ResponseDTO(ResponseCode.ERROR, new string[] { "Unable to create chat with incomplete data" }));

            Discussion chat = chatService.CreateNewChat(newChat);

            bool saved = uowService.SaveChanges();

            if (chat != null && saved)
            {
                DiscussionDTO dto = chat.GetResponseDTO();
                dto.username = dto.moderator;
                return GetJson(new ResponseDTO(ResponseCode.OK, dto));
            }
            else
                return GetJson(new ResponseDTO(ResponseCode.ERROR, new string[] { "Error creating chat" }));
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody]JoinChatDTO joinChat)
        {
            ResponseDTO result = chatService.JoinChat(joinChat);

            bool saved = uowService.SaveChanges();

            if (!saved)
                return GetJson(new ResponseDTO(ResponseCode.ERROR,
                   new string[] { "Error while joining chat" }));

            return GetJson(result);
        }

        //[Authorize]
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

        //[Authorize]
        [HttpPost("transcript")]
        public async Task<IActionResult> Download([FromBody]RequestDTO chatCode)
        {
            string doc = fileService.CreateWordDocument(chatCode.value.ToString());

            MemoryStream memory = new MemoryStream();
            using (FileStream stream = new FileStream(doc, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.ms-word", doc);
        }

        //[Authorize]
        [HttpPost("list")]
        public IActionResult GetChatList([FromBody]RequestDTO userId)
        {
            List<Discussion> chats = chatService.GetChatsByCreatorId(userId.value.ToString());

            if (chats == null || chats.Count <= 0)
                return GetJson(new ResponseDTO(ResponseCode.MESSAGE, "No chats found"));

            return GetJson(new ResponseDTO(ResponseCode.OK, chats.Select(x => x.GetResponseDTO()).ToArray()));
        }
    }
}