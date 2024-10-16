using Core.Entities.Identity;

namespace Core.Entities
{
    public class UserClass : BaseEntity
    {
        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        public string ClassId { get; set; }

        public Class Class { get; set; }
    }
}
