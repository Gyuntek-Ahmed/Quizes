using Quizes.Shared.Dtos;
using Refit;

namespace Quizes.Web.Apis.Interfaces
{
    [Headers("Authorization: Bearer ")]
    public interface IQuizApi
    {
        [Post("/api/quizes")]
        Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto);

        //Task<QuizApiResponse> DeleteQuizAsync(Guid id);
    }
}
