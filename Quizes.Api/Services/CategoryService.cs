using Microsoft.EntityFrameworkCore;
using Quizes.Api.Data;
using Quizes.Api.Data.Entities;
using Quizes.Shared.Dtos;

namespace Quizes.Api.Services
{
    public class CategoryService
    {
        private readonly QuizContext _context;

        public CategoryService(QuizContext context)
        {
            _context = context;
        }

        public async Task<QuizApiResponse> SaveCategoryAsync(CategoryDto dto)
        {
            if(await _context
                .Categories
                .AsNoTracking()
                .AnyAsync(c => c.Name == dto.Name && c.Id != dto.Id))
            {
                return QuizApiResponse.Fail("Категория с това име вече съществува.");
            }
            if (dto.Id == 0)
            {
                var category = new Category
                {
                    Name = dto.Name
                };

                _context.Categories.Add(category);
            }
            else
            {
                var dbCategory = await _context
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == dto.Id);

                if (dbCategory == null)
                    return QuizApiResponse.Fail("Категорията не е намерена.");
                

                dbCategory.Name = dto.Name;
                _context.Categories.Update(dbCategory);
            }

            await _context.SaveChangesAsync();
            return QuizApiResponse.Success();
        }

        public async Task<CategoryDto[]> GetCategoriesAsync()
            => await _context
                .Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArrayAsync();
    }
}
