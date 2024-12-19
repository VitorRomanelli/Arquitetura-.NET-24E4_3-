
namespace QuestionnaireSystem.Application.Services.Interfaces
{
    public interface IUserAnswerService
    {
        Task SaveUserAnswerAsync(int questionId, int selectedAnswerId, int userId);
    }
}