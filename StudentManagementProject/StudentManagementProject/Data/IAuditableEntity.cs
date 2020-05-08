using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Data
{
    public interface IAuditableEntity
    {
        [Required]
        Guid Id { get; set; }

        [Required]
        DateTime CreatedOnUtc { get; set; }

        [Required]
        string CreatedBy { get; set; }

        DateTime? LastModifiedOnUtc { get; set; }

        string LastModifiedBy { get; set; }
    }
}
