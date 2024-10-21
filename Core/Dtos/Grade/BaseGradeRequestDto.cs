using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Grade
{
    public class BaseGradeRequestDto
    {
        public int? Points { get; set; }

        public string Feedback { get; set; }

        [Required]
        public string HomeworkSubmissionId { get; set; }
    }
}
