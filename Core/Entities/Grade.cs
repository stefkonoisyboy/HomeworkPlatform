using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Grade : BaseEntity
    {
        public int? Points { get; set; }

        public string Feedback { get; set; }

        [Required]
        public string HomeworkSubmissionId { get; set; }

        public HomeworkSubmission HomeworkSubmission { get; set; }
    }
}
