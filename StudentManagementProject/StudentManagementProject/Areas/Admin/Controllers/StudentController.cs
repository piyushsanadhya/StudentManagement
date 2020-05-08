using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementProject.Areas.Admin.BusinessModule;
using StudentManagementProject.Areas.Admin.Models.Student;
using StudentManagementProject.Data;
using StudentManagementProject.DataTable;
using StudentManagementProject.Enum;
using StudentManagementProject.Helper;

namespace StudentManagementProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeEnumHelper(RolesEnum.Admin)]
    public class StudentController : Controller
    {
        private const string defaultPassword = "Welcome@123";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/admin/student/add-student")]
        public async Task<IActionResult> AddStudent(Guid id)
        {
            var model = new StudentViewModel();
            if (id != null && id != Guid.Empty)
            {
                var student = await GetStudentByIdAsync(id);
                model.Id = id;
                model.IsEdit = true;
                model.FirstName = student.FirstName;
                model.LastName = student.LastName;
                model.MiddleName = student.MiddleName;
                model.Class = student.Class;
                model.Section = student.Section;
                model.Email = student.ApplicationUser.Email;
            }
            return View(model);
        }

        [HttpPost]
        [Route("/admin/student/add-student")]
        public async Task<IActionResult> AddStudent(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
                if (userExist == null)
                {
                    var user = new ApplicationUser { Email = model.Email, UserName = model.Email };
                    var result = await _userManager.CreateAsync(user, defaultPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, RolesEnum.Student.ToString());
                        userExist = user;
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    if (!await _userManager.IsInRoleAsync(userExist, RolesEnum.Student.ToString()))
                    {
                        ModelState.AddModelError(string.Empty, string.Format("Cannot add user {0} to student role its present in another role", model.Email));
                        model.IsEdit = true;
                        return View(model);
                    }
                    else
                    {
                        if (model.Id == null || model.Id == Guid.Empty)
                        {
                            var student = await _context.Students.Include(s => s.ApplicationUser).Where(s => s.ApplicationUser != null && s.ApplicationUser.Id == userExist.Id).FirstOrDefaultAsync();
                            if (student != null)
                            {
                                ModelState.AddModelError(string.Empty, string.Format("The details already exists with {0} email. Please try updating the record.", model.Email));
                                model.IsEdit = true;
                                return View(model);
                            }
                        }
                    }
                }

                if (userExist != null)
                {
                    if (model.Id != null && model.Id != Guid.Empty)
                    {
                        var studentExists = await GetStudentByIdAsync(model.Id ?? Guid.Empty);
                        if (studentExists != null)
                        {
                            studentExists = SetStudentDetails(studentExists, model);
                        }
                        else
                        {
                            return RedirectToAction(nameof(StudentController.AddStudent));
                        }

                        await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                        return RedirectToAction(nameof(StudentController.AddStudent));
                    }

                    var student = new Student();

                    student = SetStudentDetails(student, model);
                    student.ApplicationUser = userExist;


                    _context.Students.Add(student);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);

                    return RedirectToAction(nameof(StudentController.AddStudent));
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("/admin/student/delete-student")]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            var student = await GetStudentByIdAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _userManager.DeleteAsync(student.ApplicationUser);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                return Json(new { Success = true, Error = false, Message = "" });
            }
            else
            {
                return Json(new { Success=false,Error=true,Message="Unable to find the student associated. Please try again later."});
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetStudents(DataTableAjaxPostModel model)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            var studentDetails = new List<StudentDetailsViewModel>();
            BlStudent blStudent = new BlStudent();
            studentDetails = await blStudent.GetStudentDetailsAsync(skip, take, sortBy + " " + (sortDir ? "asc" : "desc"), "%" + searchBy + "%");

            var count = await blStudent.GetStudentTotalDetailsCountAsync();

            return Json(new
            {
                draw = model.draw,
                recordsFiltered = count,
                recordsTotal = count,
                data = studentDetails
            });
        }

        private Student SetStudentDetails(Student student, StudentViewModel model)
        {
            student.FirstName = model.FirstName;
            student.MiddleName = model.MiddleName;
            student.LastName = model.LastName;
            student.Section = model.Section;
            student.Class = model.Class;

            return student;
        }

        private async Task<Student> GetStudentByIdAsync(Guid id)
        {
            return await _context.Students.Include(s => s.ApplicationUser).Where(s => s.Id == id).FirstOrDefaultAsync();
        }
    }
}
