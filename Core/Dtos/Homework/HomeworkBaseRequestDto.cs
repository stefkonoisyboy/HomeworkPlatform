using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Homework
{
    public class HomeworkBaseRequestDto
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        public string Instructions { get; set; }

        public int? Points { get; set; }

        public string EndDate { get; set; }

        [Required]
        public string ClassId { get; set; }
    }
}
