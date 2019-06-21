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

        private readonly UserManager<User> userManager;

        public RegistrationController(UserManager<User> userManager)
        {
            this.userManager = userManager;
           
        }
     
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = new User() {
                UserName = model.Email,
                BirthDate = model.BirthDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                RegistrationDate = DateTime.Now     
            };

            var result = await userManager.CreateAsync(userIdentity, model.Password);


            if (!result.Succeeded)
            {
               
                    return  BadRequest(new { invalid =  "Account with this email has already registred" });
                
            }
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Registration",
                new { userId = userIdentity.Id, code = code , username = userIdentity.FirstName},
                protocol: HttpContext.Request.Scheme);
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(model.Email, "Confirm your account",
                $"Confirm registration, follow : <a href='{callbackUrl}'>link</a>");

            return Ok();
          
        }

        [HttpGet]
        [HttpPost("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string username)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Error");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return BadRequest("Error");
        }
    }
}