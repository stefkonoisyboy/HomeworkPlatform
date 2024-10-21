using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.User
{
    public class EditUserDto
    {
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
