using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/member")]
    public class MemberController : BaseController
    {
        private readonly IMemberService memberService;
        private readonly IUnitOfWorkService uowService;

        public MemberController(IMemberService memberService,
            IUnitOfWorkService uowService)
        {
            this.memberService = memberService;
            this.uowService = uowService;
        }

        [HttpPost("status")]
        public IActionResult UpdateConnectionStatus([FromBody]ConnectionDetailDTO data)
        {
            memberService.UpdateConnectionStatus(data);

            bool result = uowService.SaveChanges();

            if (result)
                return GetJson(new ResponseDTO(ResponseCode.OK, data));
            else
                return GetJson(new ResponseDTO(ResponseCode.ERROR,
                    "No member found or server error"));
        }
    }
}