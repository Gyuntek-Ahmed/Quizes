using System.ComponentModel.DataAnnotations;

namespace Quizes.Shared.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Моля, въведете потребителско име."), EmailAddress, DataType(DataType.EmailAddress)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете парола.")]
        public string Password { get; set; } = null!;
    }
}
