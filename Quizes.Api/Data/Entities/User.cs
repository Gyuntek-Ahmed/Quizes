using Quizes.Shared;
using System.ComponentModel.DataAnnotations;

namespace Quizes.Api.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        [Length(10, 15)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string Role { get; set; } = nameof(UserRole.Student);


        public bool IsApproved { get; set; }
    }
}
