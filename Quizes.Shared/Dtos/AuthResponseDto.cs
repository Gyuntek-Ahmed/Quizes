using System.Text.Json.Serialization;

namespace Quizes.Shared.Dtos
{
    public record AuthResponseDto(LoggedInUser User, string? ErrorMessage = null)
    {
        [JsonIgnore]
        public bool HasError => ErrorMessage != null;
    }
}
