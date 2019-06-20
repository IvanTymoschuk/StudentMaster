using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Models;
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
        public async Task<IActionResult> Reg([FromBody]RegistrationViewModel model)
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
               
                    return new BadRequestObjectResult("Error");
                
            }

            return Ok("Account created");
        }
    }
}