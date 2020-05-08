using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Data
{
    public class Student : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string GetFullName()
        {
            return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
        }
    }
}