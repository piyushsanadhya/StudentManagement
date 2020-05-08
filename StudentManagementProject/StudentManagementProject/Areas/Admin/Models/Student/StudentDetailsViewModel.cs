using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Areas.Admin.Models.Student
{
    public class StudentDetailsViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Email { get; set; }
    }
}
