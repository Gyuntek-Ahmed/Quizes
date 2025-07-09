using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizes.Api.Data.Entities
{
    public class StudentQuiz
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual User Student { get; set; } = null!;

        public Guid QuizId { get; set; }

        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; } = null!;

        public DateTime StartedOn { get; set; }

        public DateTime CompletedOn { get; set; }

        public int Score { get; set; }
    }
}
