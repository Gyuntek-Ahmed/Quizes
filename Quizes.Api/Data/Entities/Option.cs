using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizes.Api.Data.Entities
{
    public class Option
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Text { get; set; } = null!;

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; } = null!;
    }
}
