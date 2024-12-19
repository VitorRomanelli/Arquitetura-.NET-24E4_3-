using QuestionnaireSystem.Data.Entities;

namespace QuestionnaireSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Questionnaire> GetAnsweredQuestionnaireDetailsAsync(int userId, int questionnaireId);
        Task<User> CreateUserAsync(User user);
        Task<List<Questionnaire>> GetAnsweredQuestionnairesAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
    }
}