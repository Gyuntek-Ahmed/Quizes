using System.ComponentModel.DataAnnotations;

namespace Quizes.Shared.Dtos
{
    public class QuizSaveDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Името е задължително.")]
        [MaxLength(100, ErrorMessage = "Името не може да е повече от 100 символа.")]
        public string Name { get; set; } = null!;

        [Range(1, 10000, ErrorMessage = "Категорията е задължителна.")]
        public int CategoryId { get; set; }

        [Range(1, 10000, ErrorMessage = "Моля въведете валидна стойност.")]
        public int TotalQuestions { get; set; }

        [Range(1, 120, ErrorMessage = "Моля въведете време в минути")]
        public int TimeInMinutes { get; set; }

        public bool IsActive { get; set; }

        public List<QuestionDto> Questions { get; set; } = [];

        public string Validate()
        {
            if (TotalQuestions != Questions.Count)
                return "Общият брой въпроси не съвпада с броя на въпросите в теста.";

            if (TimeInMinutes <= 0)
                return "Времето за теста трябва да бъде по-голямо от нула минути.";

            if (Questions.Any(q => string.IsNullOrWhiteSpace(q.Text)))
                return "Всички въпроси трябва да имат текст.";

            if (Questions.Any(q => q.Options.Count < 2))
                return "Всеки въпрос трябва да има поне два отговора.";

            if (Questions.Any(q => !q.Options.Any(o => o.IsCorrect)))
                return "Всеки въпрос трябва да има поне един избран отговор.";

            return string.Empty;
        }
    }
}
