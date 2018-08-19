using CentennialTalk.Models.DTOModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/auth")]
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpDTO signUpDTO)
        {
            try
            {
                IdentityUser newUser = new IdentityUser(signUpDTO.username);
                IdentityResult result = await userManager.CreateAsync(newUser, signUpDTO.password);

                if (result.Succeeded)
                {
                    return GetJson(new ResponseDTO(ResponseCode.OK, "Registeration successful"));
                }

                return GetJson(new ResponseDTO(ResponseCode.ERROR, result.Errors.ToArray()));
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