using System.ComponentModel.DataAnnotations;

namespace Quizes.Shared.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Text { get; set; } = null!;

        public List<OptionDto> Options { get; set; } = [];
    }
}
