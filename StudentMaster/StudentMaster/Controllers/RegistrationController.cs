using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Models;
using StudentMaster.Services;
using StudentMaster.ViewModels;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        private readonly AccountService accountService;

        public RegistrationController(AccountService accountService)
        {
            this.accountService = accountService;
           
        }
     
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string code = await accountService.CreateUser(model);
            if(code == null)
                return BadRequest(new { invalid = "Account with this email has already registred" });
            
            var callbackUrl = Url.Action(

                "ConfirmEmail",
                "Registration",
                new {email = model.Email, code = code },

                protocol: HttpContext.Request.Scheme);
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(model.Email, "Confirm your account",
                $"Confirm registration, follow : <a href='{callbackUrl}'>link</a>");
            return Ok();

          
        }

        [HttpGet]
        [Route("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            if (email == null || code == null)
            {
                return BadRequest("Error");
            }
            if (await accountService.FindByUserEmail(email) == null)
                return BadRequest("Error");
            if (await accountService.ConfirmEmail(await accountService.FindByUserEmail(email), code))
                return Redirect("http://localhost:3000/pickdate");
            else
                return BadRequest("Error");
        }
    }
}