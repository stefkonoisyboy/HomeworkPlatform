namespace Core.Dtos.Grade
{
    public class GradeDto
    {
        public string Id { get; set; }

        public int? Points { get; set; }

        public string Feedback { get; set; }

        public string HomeworkSubmissionId { get; set; }
    }
}
