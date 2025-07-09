using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizes.Api.Data.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Text { get; set; } = null!;

        public Guid QuizId { get; set; }

        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; } = null!;

        public virtual ICollection<Option> Options { get; set; } = [];
    }
}
