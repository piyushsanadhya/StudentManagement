using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Areas.Admin.Models.Student
{
    public class StudentViewModel
    {
        public Guid? Id { get; set; }

        public bool IsEdit { get; set; }

        [Required(ErrorMessage ="This Field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This Field is required.")]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "This Field is required.")]
        public string Section { get; set; }

        [Required(ErrorMessage = "This Field is required.")]
        public string Class { get; set; }

        [Required(ErrorMessage ="This field is required.")]
        [EmailAddress(ErrorMessage ="Please eneter a valid email.")]
        public string Email { get; set; }
    }
}
