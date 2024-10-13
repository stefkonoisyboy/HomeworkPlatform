using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
