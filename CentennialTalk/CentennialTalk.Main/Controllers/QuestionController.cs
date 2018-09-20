using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CentennialTalk.Main.Controllers
{
    [Route("api/question")]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService questionService;
        private readonly IUnitOfWorkService uowService;

        public QuestionController(IQuestionService questionService,
            IUnitOfWorkService uowService)
        {
            this.questionService = questionService;
            this.uowService = uowService;
        }

        [HttpPost("get")]
        public IActionResult GetChatQuestions([FromBody]RequestDTO chatCode)
        {
            ResponseDTO res = questionService.GetChatQuestions(chatCode.value.ToString());

            return GetJson(res);
        }

        [HttpPost("add")]
        public IActionResult AddQuestionToChat([FromBody]QuestionDTO question)
        {
            ResponseDTO res = questionService.AddQuestionToChat(question);

            bool saved = uowService.SaveChanges();

            if (!saved)
                return GetJson(new ResponseDTO(ResponseCode.ERROR, "Error saving question"));

            return GetJson(res);
        }
    }
}