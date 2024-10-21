using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.HomeworkSubmission
{
    public class CreateHomeworkSubmissionDto
    {
        [Required]
        public string HomeworkId { get; set; }

        public ICollection<IFormFile> Attachments { get; set; }
    }
}
