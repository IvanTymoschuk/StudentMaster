using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.Data
{
    public class SeedDb
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            
            context.Database.EnsureCreated();

            
            if (!context.Users.Any())
            {

                var role1 = new IdentityRole { Name = "admin" };
                var role2 = new IdentityRole { Name = "user" };


                roleManager.CreateAsync(role1);
                roleManager.CreateAsync(role2);
                
                User user = new User()
                {
                    Email = "admin@test.ua",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Major Admin"
                };
                userManager.CreateAsync(user, "admin123");

                User user2 = new User()
                {
                    Email = "test5@2aa.aa",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "lalala2"

                };
               
                userManager.CreateAsync(user2, "Password@123");

                userManager.AddToRoleAsync(user, role1.Name);
                userManager.AddToRoleAsync(user2, role2.Name);
                context.SaveChangesAsync();
            }
        }
    }
}
