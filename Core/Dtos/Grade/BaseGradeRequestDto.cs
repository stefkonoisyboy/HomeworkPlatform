using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Grade
{
    public class BaseGradeRequestDto
    {
        [Range(1, 100)]
        public int? Points { get; set; }

        public string Feedback { get; set; }

        [Required]
        public string HomeworkSubmissionId { get; set; }
    }
}
