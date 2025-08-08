namespace Quizes.Shared.Dtos
{
    public class QuizListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public int TotalQuestions { get; set; }

        public int TimeInMinutes { get; set; }

        public bool IsActive { get; set; }
    }
}
