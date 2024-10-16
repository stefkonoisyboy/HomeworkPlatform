using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Homework : BaseEntity
    {
        public Homework()
        {
            this.HomeworkSubmissions = new HashSet<HomeworkSubmission>();
        }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        public string Instructions { get; set; }

        public int? Points { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public string ClassId { get; set; }

        public Class Class { get; set; }

        public ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; }
    }
}
