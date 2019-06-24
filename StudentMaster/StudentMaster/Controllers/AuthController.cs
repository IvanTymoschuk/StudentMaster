using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var user = await userManager.FindByNameAsync(model.Email);
            if(user !=null && await userManager.CheckPasswordAsync(user,model.Password))
            {
                
                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    
                    return BadRequest(new { invalid = "You didn`t confirm your email" });
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
                    claims:claims,

                    signingCredentials:new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256)
                    
                    );
                return  Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                });
            }
            return BadRequest(new { invalid = "Email or password is not correct" });
        }
    }
}