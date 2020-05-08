using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementProject.Areas.Students.Models.Profile;
using StudentManagementProject.Data;
using StudentManagementProject.Enum;
using StudentManagementProject.Helper;

namespace StudentManagementProject.Areas.Students.Controllers
{
    [Area("Students")]
    [AuthorizeEnumHelper(RolesEnum.Student)]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ViewProfile()
        {
            var student = await GetCurrentStudentAsync();
            if (student!=null)
            {
                var model = new StudentViewModel()
                {
                    Class = student.Class,
                    Section = student.Section,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    MiddleName = student.MiddleName
                };
                return View(model);
            }
            else
            {
                return RedirectToPage("./");
            }
        }

        private async Task<Student> GetCurrentStudentAsync()
        {
            var user = await GetCurrentUserAsync();
            return await _context.Students.Include(s=>s.ApplicationUser).Where(s => s.ApplicationUser != null && s.ApplicationUser.Id == user.Id).FirstOrDefaultAsync();
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _context.Users.Where(u => u.Email == HttpContext.User.Identity.Name).FirstOrDefaultAsync();
        }
    }
}
