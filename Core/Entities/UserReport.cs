using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class UserReport : BaseEntity
    {
        [Required]
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        [Required]
        public string ReportId { get; set; }

        public Report Report { get; set; }
    }
}
