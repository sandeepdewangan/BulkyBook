using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                // Migrate pending migrations
                if(_dbContext.Database.GetPendingMigrations().Count () > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            // Everytime the application starts it will keep on creating the roles. so to avoid.
            if (_dbContext.Roles.Any(r => r.Name == SD.Role_Admin)) return;

            // Creating roles on the go, awaiter is used to stop the code here until it get results.
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();

            // Creating user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "sandeep@gmail.com",
                Email = "sandeep@gmail.com",
                EmailConfirmed = true,
                Name = "Sandeep Dewangan"
            }, "Sandeep123@").GetAwaiter().GetResult();

            // Get Admin User
            ApplicationUser user = _dbContext.ApplicationUsers.Where(u => u.Email == "sandeep@gmail.com").FirstOrDefault();

            // Assign the user to role of ADMIN
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
