using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentMaster.Data;
using StudentMaster.Models;
using StudentMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StudentMaster.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext appDbContext;
        private readonly UserManager<User> userManager;
        public AccountService(UserManager<User> userManager, ApplicationDbContext appDbContext)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
        }

        public async Task<string> GetTokenSocialLogin(SocialLoginViewModel model)
        {
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
                    return null;
                }
            }

            JwtSecurityToken token = GenerateToken(user);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<StudyInfoViewModel> GetStudyInfo(string id)
        {
            StudyInfoViewModel model = new StudyInfoViewModel();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            model.studyDate = user.StudyDate;
            model.tillEnd = (user.StudyDate - DateTime.Now).Days;
            return model;
        }

        public async Task<IdentityResult> ResetPassword (ResetPasswordViewModel model) {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return null;
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            return result;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return false;
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("", "resetpassword", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            //EmailService emailService = new EmailService();
            //await emailService.SendEmailAsync(model.Email, "Reset Password",
            //    $"For reset password - follow: <a href='{callbackUrl}'>link</a>");
            return true;
        }
        public async Task PickStudyDate(StudyDateViewModel model)
        {

            var user = await userManager.FindByIdAsync(model.UserId);

            user.StudyDate = model.StudyDate;
            appDbContext.Entry(user).State = EntityState.Modified;
            appDbContext.SaveChanges();


                    BackgroundJob.Schedule(
             () => SendNotification(user),
             user.StudyDate.AddMonths(-1));


                    BackgroundJob.Schedule(
             () => SendNotification(user),
             user.StudyDate.AddDays(-7));


                    BackgroundJob.Schedule(
             () => SendNotification(user),
             user.StudyDate.AddDays(-1).Date + new TimeSpan(7, 0, 0));
        }
        public async Task SendNotification(User user)
        {

            //var callbackUrl = Url.Action("", "schedule", new { }, protocol: HttpContext.Request.Scheme);
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Notification",
                $"Your studing will start: ");
        }
        private JwtSecurityToken GenerateToken(User user)
        {
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
            return token;
        }


    }
}
