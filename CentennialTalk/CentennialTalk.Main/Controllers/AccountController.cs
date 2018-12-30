using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/auth")]
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.emailService = emailService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpDTO signUpDTO)
        {
            try
            {
                IdentityUser newUser = new IdentityUser(signUpDTO.username);
                newUser.Email = signUpDTO.email;
                IdentityResult result = await userManager.CreateAsync(newUser, signUpDTO.password);

                if (result.Succeeded)
                {
                    string evc = userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;

                    string url = string.Format("{0}://{1}{2}/{3}?userId={4}&code={5}"
                        , Request.Scheme
                        , Request.Host
                        , Request.PathBase
                        , "verify"
                        , newUser.Id, evc);

                    string apikey = configuration["SendGridAPIKey"];

                    Response res = await emailService.SendEmail(apikey, "Confirm your email",
                        $"Hello " + newUser.UserName + ", Please reset your password by <a href='" + HtmlEncoder.Default.Encode(url) + "'>clicking here</a>.",
                        newUser.Email);

                    return GetJson(new ResponseDTO(ResponseCode.OK, "Registeration successful"));
                }

                return GetJson(new ResponseDTO(ResponseCode.ERROR, result.Errors.ToArray()));
            }
            catch (Exception ex)
            {
                return GetJson(new ResponseDTO(ResponseCode.ERROR, ex.Message));
            }
        }

        [HttpPost("resetlink")]
        public async Task<IActionResult> SendResetLink([FromBody]RequestDTO email)
        {
            try
            {
                IdentityUser newUser = await userManager.FindByEmailAsync(email.value.ToString());

                if (newUser == null)
                    return GetJson(new ResponseDTO(ResponseCode.ERROR, "User with the supplied email not found"));

                string prc = userManager.GeneratePasswordResetTokenAsync(newUser).Result;

                string url = string.Format("{0}://{1}{2}/{3}?userId={4}&code={5}"
                    , Request.Scheme
                    , Request.Host
                    , Request.PathBase
                    , "reset"
                    , newUser.Id, prc);

                string apikey = configuration["SendGridAPIKey"];

                Response res = await emailService.SendEmail(apikey, "Reset your password",
                    $"Hello " + newUser.UserName + ", Please reset your password by <a href='" + HtmlEncoder.Default.Encode(url) + "'>clicking here</a>.",
                    newUser.Email);

                return GetJson(new ResponseDTO(ResponseCode.OK, "Link sent succesfully"));
            }
            catch (Exception ex)
            {
                return GetJson(new ResponseDTO(ResponseCode.ERROR, ex.Message));
            }
        }

        public class EmailVer
        {
            public string userId;
            public string token;
            public string password;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody]EmailVer obj)
        {
            try
            {
                IdentityUser newUser = await userManager.FindByIdAsync(obj.userId);

                IdentityResult result = await userManager.ConfirmEmailAsync(newUser, obj.token);

                if (result.Succeeded)
                {
                    return GetJson(new ResponseDTO(ResponseCode.OK, "Email confirmed successfully"));
                }
                else
                {
                    return GetJson(new ResponseDTO(ResponseCode.ERROR, result.Errors.ToArray()));
                }
            }
            catch (Exception ex)
            {
                return GetJson(new ResponseDTO(ResponseCode.ERROR, ex.Message));
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody]EmailVer obj)
        {
            try
            {
                IdentityUser newUser = await userManager.FindByIdAsync(obj.userId);

                IdentityResult result = await userManager.ResetPasswordAsync(newUser, obj.token, obj.password);

                if (result.Succeeded)
                {
                    return GetJson(new ResponseDTO(ResponseCode.OK, "Password reset successfully"));
                }
                else
                {
                    return GetJson(new ResponseDTO(ResponseCode.ERROR, result.Errors.ToArray()));
                }
            }
            catch (Exception ex)
            {
                return GetJson(new ResponseDTO(ResponseCode.ERROR, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]SignUpDTO login)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager
                    .PasswordSignInAsync(login.username, login.password, false, false);

            if (result.Succeeded)
            {
                return GetJson(new ResponseDTO(ResponseCode.OK, GenerateJwtToken(login.username)));
            }

            return GetJson(new ResponseDTO(ResponseCode.ERROR, "Invalid login attempt"));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return GetJson(new ResponseDTO(ResponseCode.OK, "Logged out successfully"));
        }

        private string GenerateJwtToken(string username)//, IdentityUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtExpireDays"]));

            JwtSecurityToken token = new JwtSecurityToken
            (
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}