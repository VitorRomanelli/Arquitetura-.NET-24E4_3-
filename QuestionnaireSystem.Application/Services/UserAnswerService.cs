using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;

namespace QuestionnaireSystem.Application.Services
{
    public class UserAnswerService(IUserAnswerRepository repository) : IUserAnswerService
    {
        private readonly IUserAnswerRepository _repository = repository;

        public async Task SaveUserAnswerAsync(int questionId, int selectedAnswerId, int userId)
        {
            var userAnswer = new UserAnswer
            {
                QuestionId = questionId,
                SelectedAnswerId = selectedAnswerId,
                UserId = userId
            };

            await _repository.SaveUserAnswerAsync(userAnswer);
        }
    }
}
