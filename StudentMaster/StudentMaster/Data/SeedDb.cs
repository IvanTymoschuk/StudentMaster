using Microsoft.AspNetCore.Identity;
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
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                User user = new User()
                {
                    Email = "test@aa.aa",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "lalala"
                };
                userManager.CreateAsync(user, "Password@123");

                User user2 = new User()
                {
                    Email = "test5@2aa.aa",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "lalala2"

                };
                userManager.CreateAsync(user2, "Password@123");
            }
        }
    }
}
