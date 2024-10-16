﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.CreatedClasses = new HashSet<Class>();
            this.EnrolledClasses = new HashSet<UserClass>();
            this.HomeworkSubmissions = new HashSet<HomeworkSubmission>(); 
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Class> CreatedClasses { get; set; }

        public ICollection<UserClass> EnrolledClasses { get; set; }

        public ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; }
    }
}
