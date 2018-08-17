using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/chat")]
    public class DiscussionController : BaseController
    {
        private readonly IChatService chatService;
        private readonly IUnitOfWorkService uowService;

        public DiscussionController(IChatService chatService,
            IUnitOfWorkService uowService)
        {
            this.chatService = chatService;
            this.uowService = uowService;
        }

        [HttpPost("new")]
        public IActionResult New([FromBody]NewChatDTO newChat)
        {
            Discussion chat = chatService.CreateNewChat(newChat);

            uowService.SaveChanges();

            if (chat != null)
                return GetJson(new ResponseDTO(ResponseCode.OK, chat));
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
        public IActionResult CloseJoiningLink(string chatCode)
        {
            Discussion chat = chatService.GetChatByCode(chatCode);

            if (chat == null)
                return GetJson(new ResponseDTO(ResponseCode.ERROR,
                    "No Chat with such code exists"));

            if (!chat.IsLinkOpen)
                return GetJson(new ResponseDTO(ResponseCode.OK, "Chat is already closed"));

            chat.IsLinkOpen = false;

            uowService.SaveChanges();

            return GetJson(new ResponseDTO(ResponseCode.OK, "Success"));
        }

        [HttpPost("questions/add")]
        public IActionResult AddQuestionToChat([FromBody]QuestionDTO question)
        {
            ResponseDTO res = chatService.AddQuestionToChat(question);

            bool saved = uowService.SaveChanges();

            if (!saved)
                return GetJson(new ResponseDTO(ResponseCode.ERROR, "Error saving question"));

            return GetJson(res);
        }
    }
}