namespace Core.Dtos.Homework
{
    public class HomeworkDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Instructions { get; set; }

        public int? Points { get; set; }

        public DateTime? EndDate { get; set; }

        public string ClassId { get; set; }
    }
}
