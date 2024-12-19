using QuestionnaireSystem.Data.Entities;

namespace QuestionnaireSystem.Persistence.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<Questionnaire> GetAnsweredQuestionnaireDetailsAsync(int userId, int questionnaireId);
        Task<List<Questionnaire>> GetAnsweredQuestionnairesAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
    }
}