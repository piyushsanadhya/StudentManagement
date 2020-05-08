using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Data
{
    public class AuditableEntity : IAuditableEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedOnUtc { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedOnUtc { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
