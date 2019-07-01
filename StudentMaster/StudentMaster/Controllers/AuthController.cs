using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Services;
using StudentMaster.ViewModels;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AccountService accountService;

        public AuthController(AccountService accountService)
        {
            this.accountService = accountService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to login with social network");
            }

            if (!await accountService.IsEmailConfirmed(model.Email))
            {
                return BadRequest(new { invalid = "Email is not confirmed" });

            }

            var token = await this.accountService.GetTokenLogin(model);
            if (token == null)
            {
                return BadRequest(new { invalid = "Login or password is incorrect" });
            }
            else
            {
                return Ok(new
                {
                    token
                });
            }
        }
          
    }
}