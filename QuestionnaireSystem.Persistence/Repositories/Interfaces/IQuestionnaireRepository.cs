using QuestionnaireSystem.Data.Entities;

namespace QuestionnaireSystem.Persistence.Repositories.Interfaces
{
    public interface IQuestionnaireRepository
    {
        Task<Questionnaire?> GetQuestionnaireWithDetailsAsync(int questionnaireId);
        Task AddAsync(Questionnaire questionnaire);
        Task<List<Questionnaire>> GetAllAsync();
        Task<Questionnaire> GetByIdAsync(int id);
    }
}