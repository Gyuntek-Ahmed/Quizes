using Quizes.Shared.Dtos;
using Refit;

namespace Quizes.Web.Apis.Interfaces
{
    public interface IAuthApi
    {
        [Post("/api/auth/login")]
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
