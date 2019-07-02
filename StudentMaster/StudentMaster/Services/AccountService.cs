using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentMaster.Data;
using StudentMaster.Models;
using StudentMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public async Task<EditUserViewModel> GetUserData(string userid)
        {

            var user = await FindByUserId(userid);
            if (user == null)
                return null;
            

            EditUserViewModel model = new EditUserViewModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email

            };
            return  model;
        }
        public async Task<bool> IsEmailConfirmed(string Email)
        {
            var user = await userManager.FindByNameAsync(Email);
            if (!await userManager.IsEmailConfirmedAsync(user))
            {

                return false;
            }
            return true;
        }

        public async Task<string> GetTokenLogin(LoginViewModel model)
        {

            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null)
                return "";
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                JwtSecurityToken token = GenerateToken(user);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;       
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

        public async Task<IdentityResult> ResetPassword (ResetPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return null;
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            return result;
        }

        public async Task<string> CreateUser (RegistrationViewModel model)
        {
            var userIdentity = new User()
            {
                UserName = model.Email,
                BirthDate = model.BirthDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                RegistrationDate = DateTime.Now
            };

            var result = await userManager.CreateAsync(userIdentity, model.Password);
            await userManager.AddToRoleAsync(userIdentity, "user");

            if (!result.Succeeded)
            {

                return null;

            }
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            return code;
        }

        public async Task<string> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return null;
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
           
            return code;
        }
        public async Task PickStudyDate(StudyDateViewModel model)
        {

            var user = await userManager.FindByIdAsync(model.UserId);

            user.StudyDate = model.StudyDate.AddDays(1);
            appDbContext.Entry(user).State = EntityState.Modified;
            appDbContext.SaveChanges();

            if (user.StudyDate.AddMonths(-1) >= DateTime.Now)
            {
                BackgroundJob.Schedule(
                () => SendNotification(user),
                user.StudyDate.AddMonths(-1));
            }
            if (user.StudyDate.AddDays(-7) >= DateTime.Now)
            {
                BackgroundJob.Schedule(
                () => SendNotification(user),
                user.StudyDate.AddDays(-7));
            }

            if (user.StudyDate.AddDays(-1).Date + new TimeSpan(7, 0, 0) >= DateTime.Now)
            {
                BackgroundJob.Schedule(
                () => SendNotification(user),
                user.StudyDate.AddDays(-1).Date + new TimeSpan(7, 0, 0));
            }
        }
        public async Task<bool> ConfirmEmail(User user, string code)
        {
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<User> FindByUserId(string UserId)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
                return null;
            return user;
        }

        public async Task<User> FindByUserEmail(string UserId)
        {
            var user = await userManager.FindByEmailAsync(UserId);
            return user;
        }
        public async Task SendNotification(User user)
        {

          
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Notification",
                $"Your studing will start in: "+ (user.StudyDate - DateTime.Now).Days +" days {"+user.StudyDate.ToShortDateString()+"}");
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
