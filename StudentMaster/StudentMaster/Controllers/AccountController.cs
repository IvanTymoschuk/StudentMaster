using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentMaster.Services;
using StudentMaster.ViewModels;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService accountService;
        public AccountController(AccountService accountService)
        {
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