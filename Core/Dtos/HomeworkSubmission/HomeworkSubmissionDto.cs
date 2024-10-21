namespace Core.Dtos.HomeworkSubmission
{
    public class HomeworkSubmissionDto
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? GradePoints { get; set; }

        public int? HomeworkPoints { get; set; }

        public string HomeworkTitle { get; set; }

        public string HomeworkInstructions { get; set; }

        public string StudentId { get; set; }

        public string StudentFullName { get; set; }
    }
}
