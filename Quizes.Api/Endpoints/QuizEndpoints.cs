using Quizes.Api.Services;
using Quizes.Shared.Dtos;

namespace Quizes.Api.Endpoints
{
    public static class QuizEndpoints
    {
        public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder app)
        {
            var quizGroup = app.MapGroup("/api/quizes").RequireAuthorization();

            quizGroup.MapPost("/", async (QuizSaveDto dto, QuizService service) =>
            {
                if(dto.Questions.Count == 0)
                    return Results.BadRequest( "Тестът трябва да съдържа поне един въпрос.");

                if(dto.Questions.Count != dto.TotalQuestions)
                    return Results.BadRequest("Броят на въпросите в теста не съвпада с общия брой въпроси.");

                return Results.Ok(await service.SaveQuizAsync(dto));
            });

            return app;
        }
    }
}
