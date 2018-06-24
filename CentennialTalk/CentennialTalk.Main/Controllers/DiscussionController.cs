using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/chat")]
    public class DiscussionController : Controller
    {
        private readonly IChatService chatService;

        public DiscussionController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("new")]
        public IActionResult New([FromBody]NewChatDTO newChat)
        {
            Discussion chat = chatService.CreateNewChat(newChat.moderator, newChat.title);

            return new JsonResult(chat);
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody]JoinChatDTO joinChat)
        {
            Discussion chat = chatService.GetChatByCode(joinChat.chatCode);

            return new JsonResult(chat);
        }
    }
}