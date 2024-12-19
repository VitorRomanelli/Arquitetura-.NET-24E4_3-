using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Application.Services
{
    public class UserService(IUserRepository repository) : IUserService
    {
        private readonly IUserRepository _repository = repository;

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _repository.GetUserByIdAsync(userId);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("User name and email are required.");

            var existingUser = await _repository.GetUserByIdAsync(user.Id);
            if (existingUser != null)
                throw new InvalidOperationException("A user with the same ID already exists.");

            return await _repository.CreateUserAsync(user);
        }

        public async Task<List<Questionnaire>> GetAnsweredQuestionnairesAsync(int userId)
        {
            return await _repository.GetAnsweredQuestionnairesAsync(userId);
        }

        public async Task<Questionnaire> GetAnsweredQuestionnaireDetailsAsync(int userId, int questionnaireId)
        {
            return await _repository.GetAnsweredQuestionnaireDetailsAsync(userId, questionnaireId);
        }
    }
}
