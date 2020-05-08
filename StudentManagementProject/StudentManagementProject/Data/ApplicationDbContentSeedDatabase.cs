using Microsoft.AspNetCore.Identity;
using StudentManagementProject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Data
{
    public static class ApplicationDbContentSeedDatabase
    {
        private const string _defaultPassword = "Password123!";
        private const string _defaultAdminUserName = "appadmin@example.com";
        private const string _defaultAdminEmail = "appadmin@example.com";


        private static readonly string _studentRole = RolesEnum.Student.ToString();
        private static readonly string _adminRole = RolesEnum.Admin.ToString();

        public async static Task EnsureSeedDataAsync(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            if (!(await _roleManager.RoleExistsAsync(_adminRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(_adminRole));
            }

            if (!(await _roleManager.RoleExistsAsync(_studentRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(_studentRole));
            }

            var user = await _userManager.FindByNameAsync(_defaultAdminUserName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = _defaultAdminUserName,
                    Email = _defaultAdminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, _defaultPassword);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, _adminRole);
                }
            }
        }
    }
}