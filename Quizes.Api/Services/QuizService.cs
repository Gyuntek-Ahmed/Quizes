using Microsoft.EntityFrameworkCore;
using Quizes.Api.Data;
using Quizes.Api.Data.Entities;
using Quizes.Shared.Dtos;

namespace Quizes.Api.Services
{
    public class QuizService
    {
        private readonly QuizContext _context;

        public QuizService(QuizContext context)
        {
            _context = context;
        }

        public async Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto)
        {
            var questions = dto.Questions
                    .Select(q => new Question
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Options = [.. q.Options.Select(o => new Option
                    {
                        Id = o.Id,
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    })]
                    }).ToArray();

            if (dto.Id == Guid.Empty)
            {
                // Create new quiz

                var quiz = new Quiz
                {
                    CategoryId = dto.CategoryId,
                    IsActive = dto.IsActive,
                    Name = dto.Name,
                    TimeInMinutes = dto.TimeInMinutes,
                    TotalQuestions = dto.TotalQuestions,
                    Questions = questions
                };

                _context.Quizzes.Add(quiz);
            }
            else
            {
                // Update existing quiz
                var existingQuiz = await _context.Quizzes
                    .FirstOrDefaultAsync(q => q.Id == dto.Id);

                if (existingQuiz == null)
                    return QuizApiResponse.Fail("Тестът не съществува.");

                existingQuiz.Name = dto.Name;
                existingQuiz.CategoryId = dto.CategoryId;
                existingQuiz.TimeInMinutes = dto.TimeInMinutes;
                existingQuiz.TotalQuestions = dto.TotalQuestions;
                existingQuiz.IsActive = dto.IsActive;
                existingQuiz.Questions = questions;

                _context.Quizzes.Update(existingQuiz);
            }

            try
            {
                await _context.SaveChangesAsync();
                return QuizApiResponse.Success();
            }
            catch (Exception ex)
            {
                return QuizApiResponse.Fail($"Грешка при запазване на теста: {ex.Message}");
            }
        }

        public async Task<QuizListDto[]> GetQuizesAsync()
        {
            // Assignment:Implemetn Paging and Server Side Filtering
            return await _context.Quizzes.Select(q => new QuizListDto
            {
                Id = q.Id,
                Name = q.Name,
                CategoryId = q.CategoryId,
                CategoryName = q.Category.Name,
                TotalQuestions = q.TotalQuestions,
                TimeInMinutes = q.TimeInMinutes,
                IsActive = q.IsActive
            }).ToArrayAsync();
        }

        public async Task<QuestionDto[]> GetQuizQuestionsAsync(Guid quizId)
            => await _context.Questions
                .Where(q => q.QuizId == quizId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text
                }).ToArrayAsync();

        public async Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId)
        {
            var quiz = await _context.Quizzes
                .Where(q => q.Id == quizId)
                .Select(qz => new QuizSaveDto
                {
                    Id = qz.Id,
                    Name = qz.Name,
                    CategoryId = qz.CategoryId,
                    TotalQuestions = qz.TotalQuestions,
                    TimeInMinutes = qz.TimeInMinutes,
                    IsActive = qz.IsActive,
                    Questions = qz.Questions.Select(qs => new QuestionDto
                    {
                        Id = qs.Id,
                        Text = qs.Text,
                        Options = qs.Options.Select(o => new OptionDto
                        {
                            Id = o.Id,
                            Text = o.Text,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();

            return quiz;
        }
    }
}
