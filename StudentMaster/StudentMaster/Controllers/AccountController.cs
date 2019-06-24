using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentMaster.Data;
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
        private readonly ApplicationDbContext _appDbContext;
        public AccountController(UserManager<User> userManager, ApplicationDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
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
                await userManager.AddToRoleAsync(user, "user");
                if (!result.Succeeded)
                {
                    return BadRequest(new { invalid = "Something went wrong!" });
                }
            }
           
            var roles = userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>()
                {

                new Claim("id", user.Id),
                new Claim("name", user.UserName),
                };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
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
        [Route("pickstudydate")]
        public async Task<IActionResult> PickStudyDate([FromBody]StudyDateViewModel model)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(model);
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest(new { invalid = "UserId is not registred in system" });
            }
            user.StudyDate = model.StudyDate;
            _appDbContext.Entry(user).State = EntityState.Modified;
            _appDbContext.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("testrole")]
        public ActionResult About()
        {
            

            return Ok(new { add = "sa" });
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