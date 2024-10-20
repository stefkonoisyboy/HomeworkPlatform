using Microsoft.AspNetCore.Http;

namespace Core.Dtos.Homework
{
    public class CreateHomeworkDto : HomeworkBaseRequestDto
    {
        public ICollection<IFormFile> Attachments { get; set; }
    }
}
