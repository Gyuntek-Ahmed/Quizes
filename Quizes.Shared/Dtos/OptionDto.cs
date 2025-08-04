using System.ComponentModel.DataAnnotations;

namespace Quizes.Shared.Dtos
{
    public class OptionDto
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Text { get; set; } = null!;

        public bool IsCorrect { get; set; }
    }
}
