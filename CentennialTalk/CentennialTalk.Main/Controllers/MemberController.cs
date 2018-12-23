using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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

        [HttpPost("members")]
        public IActionResult GetChatMembers([FromBody]RequestDTO chatCode)
        {
            List<GroupMember> gMembers = memberService.GetChatMembers(chatCode.value.ToString());

            if (gMembers == null || gMembers.Count <= 0)
                return GetJson(new ResponseDTO(ResponseCode.OK, new List<string>()));

            return GetJson(new ResponseDTO(ResponseCode.OK, gMembers.Select(x => x.Username).Distinct().ToArray()));
        }
    }
}