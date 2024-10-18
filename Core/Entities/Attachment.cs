using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Attachment : BaseEntity
    {
        [Required]
        [Url]
        public string AttachmentUri { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public string HomeworkId { get; set; }

        public Homework Homework { get; set; }

        public string HomeworkSubmissionId { get; set; }

        public HomeworkSubmission HomeworkSubmission { get; set; }
    }
}
