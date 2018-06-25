using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/chat")]
    public class DiscussionController : Controller
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
            Discussion chat = chatService.CreateNewChat(newChat.moderator, newChat.title);

            uowService.SaveChanges();

            return new ResponseDTO(ResponseCode.OK, chat).Json;
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody]JoinChatDTO joinChat)
        {
            Discussion chat = chatService.GetChatByCode(joinChat.chatCode);

            uowService.SaveChanges();

            return new ResponseDTO(ResponseCode.OK, chat).Json;
        }
    }
}