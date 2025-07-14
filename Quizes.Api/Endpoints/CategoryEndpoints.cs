using Quizes.Api.Services;
using Quizes.Shared;
using Quizes.Shared.Dtos;

namespace Quizes.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var categoryGroup = app
                .MapGroup("/api/categories")
                .RequireAuthorization();

            categoryGroup.MapGet("", async(CategoryService categoryService)
                => Results.Ok(await categoryService.GetCategoriesAsync()));

            categoryGroup.MapPost("", async (CategoryDto dto, CategoryService categoryService)
                => Results
                .Ok(await categoryService.SaveCategoryAsync(dto)))
                .RequireAuthorization(p => p.RequireRole(nameof(UserRole.Admin)));

            return app;
        }
    }
}
