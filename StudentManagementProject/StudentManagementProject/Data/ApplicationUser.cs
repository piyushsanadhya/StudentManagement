using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }

        public DateTime CreatedOnUtc { get; set; }
    }
}
