using System.ComponentModel.DataAnnotations;

namespace Quizes.Shared.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително."), MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
