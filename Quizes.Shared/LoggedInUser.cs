using System.Security.Claims;
using System.Text.Json;

namespace Quizes.Shared
{
    public record LoggedInUser(int Id, string Name, string Role, string Token)
    {
        public string ToJson() => JsonSerializer.Serialize(this);

        public static LoggedInUser? FromJson(string json) =>
            !string.IsNullOrEmpty(json)
            ? JsonSerializer.Deserialize<LoggedInUser>(json)
            :null;


        public Claim[] ToClaims()
        {
            return
            [
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, Role),
                new Claim(nameof(Token), Token)
            ];
        }
    }
}
