using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
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
        private readonly AccountService accountService;
        public AccountController(UserManager<User> userManager, ApplicationDbContext appDbContext, AccountService accountService)
        {
            this._appDbContext = appDbContext;
            this.userManager = userManager;
            this.accountService = accountService;
        }

        [HttpPost("sociallogin")]
        public async Task<IActionResult> SocialNetworkLogin([FromBody]SocialLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to login with social network");
            }
            var token = await this.accountService.GetTokenSocialLogin(model);
            if (token == null)
            {
                return BadRequest(new { invalid = "Something went wrong" });
            }
            else
            {
                return Ok(new
                {
                    token
                });
            }
        }


        [HttpPost]
        [Route("resetpassword")]

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (accountService.ResetPassword(model).Result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { invalid = "UserId is not registred in system" });
            }
        }

        [HttpPost]
        [Route("pickstudydate")]
        public async Task<IActionResult> PickStudyDate([FromBody]StudyDateViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(model);
            await accountService.PickStudyDate(model);
            return Ok();
        }

       

        [HttpGet]
        [Route("studyinfo")]
        public async Task<IActionResult> StudyInfo([FromQuery]string id)
        {
            var result = await accountService.GetStudyInfo(id);
           if (result!=null)
            return Ok(result);
           else return BadRequest(new { invalid = "UserId is not registred in system" });
        }

        [HttpPost]
        [Route("forgotpassword")]

        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
               if(await accountService.ForgotPassword(model))
                return Ok();
               else
                 return BadRequest(new { invalid = "This email is not registred in system" });
            }
            return BadRequest(ModelState);
        }
    }
}