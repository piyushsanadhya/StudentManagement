using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Areas.Students.Models.Profile
{
    public class StudentViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
    }
}
