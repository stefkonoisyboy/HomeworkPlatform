using Core.Entities.Enums;
using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Report : BaseEntity
    {
        public Report()
        {
            this.Students = new HashSet<UserReport>();
        }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public ReportStatus Status { get; set; }

        [Required]
        public string TeacherId { get; set; }

        public ApplicationUser Teacher { get; set; }

        public ICollection<UserReport> Students { get; set; }
    }
}
