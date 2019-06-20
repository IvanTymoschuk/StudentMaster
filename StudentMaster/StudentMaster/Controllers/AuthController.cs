using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentMaster.Models;
using StudentMaster.ViewModels;

namespace StudentMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<User> userManager;

        public AuthController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if(user !=null && await userManager.CheckPasswordAsync(user,model.Password))
            {
                // проверяем, подтвержден ли email
                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    
                    return BadRequest("You didn`t confirm your email");
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                };
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));

                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(1),
                    claims:claims,
                    issuer: "http://test.com",
                    audience: "http://test.com",
                    signingCredentials:new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256)
                    
                    );
                return  Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                    
                });
            }
            return Unauthorized();
        }
    }
}