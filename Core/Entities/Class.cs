using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Class : BaseEntity
    {
        public Class()
        {
            this.Students = new HashSet<UserClass>();
            this.Homeworks = new HashSet<Homework>();
        }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public string LogoUri { get; set; }

        [Required]
        public string TeacherId { get; set; }

        public ApplicationUser Teacher { get; set; }

        public ICollection<UserClass> Students { get; set; }

        public ICollection<Homework> Homeworks { get; set; }
    }
}
