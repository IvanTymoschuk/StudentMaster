using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentMaster.Models;
using StudentMaster.Services;
using StudentMaster.ViewModels;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<User> userManager;

        public AccountController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("sociallogin")]
        public async Task<IActionResult> SocialNetworkLogin([FromBody]SocialLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to login with social network");
            }

            var user = userManager.FindByEmailAsync(model.Email).Result;
            if (user == null)
            {
                user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.Name
                };
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new { invalid = "Something went wrong!" });
                }
            }
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
                issuer: "http://test.com",
                audience: "http://test.com",
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }


        [HttpPost]
        [Route("resetpassword")]

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {


                var user = await userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return BadRequest(new { invalid = "UserId is not registred in system" });
                }
                var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return BadRequest(ModelState);

        }

        [HttpPost]
        [Route("forgotpassword")]

        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return BadRequest(new { invalid = "This email is not registred in system" });
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("", "resetpassword", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"For reset password - follow: <a href='{callbackUrl}'>link</a>");
                return Ok(new { answer = "Check your email" });
            }
            return BadRequest(ModelState);
        }
    }
}