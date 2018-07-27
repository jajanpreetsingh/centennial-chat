using CentennialTalk.Models.DTOModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/auth")]
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpDTO signUpDTO)
        {
            IdentityUser newUser = new IdentityUser(signUpDTO.username);
            IdentityResult result = await userManager.CreateAsync(newUser, signUpDTO.password);

            if (result.Succeeded)
            {
                return GetJson(new ResponseDTO(ResponseCode.OK, "Registeration successful"));
            }

            return GetJson(new ResponseDTO(ResponseCode.ERROR, result.Errors.ToArray()));
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody]NewChatDTO newChatDTO)
        {
            IdentityResult result = await signInManager.SignInAsync()

            if (result.Succeeded)
            {
                return GetJson(new ResponseDTO(ResponseCode.OK, "Registeration successful"));
            }

            return GetJson(new ResponseDTO(ResponseCode.ERROR, result.Errors.ToArray()));
        }
    }
}