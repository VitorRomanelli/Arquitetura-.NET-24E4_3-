using Microsoft.EntityFrameworkCore;
using QuestionnaireSystem.Data.Entities;

namespace QuestionnaireSystem.Persistence.Repositories.Interfaces
{
    public interface IUserAnswerRepository
    {
        Task SaveUserAnswerAsync(UserAnswer userAnswer);
        Task SaveUserAnswerRangeAsync(List<UserAnswer> userAnswers);
    }
}