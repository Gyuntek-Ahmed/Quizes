using Quizes.Shared.Dtos;
using Refit;

namespace Quizes.Web.Apis.Interfaces
{
    [Headers("Authorization: Bearer ")]
    public interface IQuizApi
    {
        [Post("/api/quizes")]
        Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto);

        [Get("/api/quizes")]
        Task<QuizListDto[]> GetQuizesAsync();

        [Get("/api/quizes/{quizId}/questions")]
        Task<QuestionDto[]> GetQuizQuestionsAsync(Guid quizId);

        [Get("/api/quizes/{quizId}")]
        Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId);
    }
}
