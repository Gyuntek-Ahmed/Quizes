using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizes.Api.Data.Entities
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        public int TotalQuestions { get; set; }

        public int TimeInMinutes { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Question> Questions { get; set; } = [];
    }
}
