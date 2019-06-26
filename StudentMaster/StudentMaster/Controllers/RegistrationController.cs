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

           if(!await accountService.CreateUser(model))
                return BadRequest(new { invalid = "Account with this email has already registred" });

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
            if (await accountService.FindByUserId(userId) == null)
                return BadRequest("Error");
            if(await accountService.ConfirmEmail(await accountService.FindByUserId(userId), code))
                return RedirectToAction("Index", "Home");
            else
                return BadRequest("Error");
        }
    }
}