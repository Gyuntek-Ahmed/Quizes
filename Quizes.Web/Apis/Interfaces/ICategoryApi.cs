using Quizes.Shared.Dtos;
using Refit;

namespace Quizes.Web.Apis.Interfaces
{
    [Headers("Authorization: Bearer ")]
    public interface ICategoryApi
    {
        [Post("/api/categories")]
        Task<QuizApiResponse> SaveCategoriesAsync(CategoryDto categoryDto);

        [Get("/api/categories")]
        Task<CategoryDto[]> GetCategoriesAsync();
    }
}
