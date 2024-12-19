using Microsoft.EntityFrameworkCore;
using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.UserAnswers)
                                       .ThenInclude(ua => ua.Question)
                                       .ThenInclude(q => q.Answers)
                                       .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<Questionnaire>> GetAnsweredQuestionnairesAsync(int userId)
        {
            return await _context.UserAnswers
                                 .Where(ua => ua.UserId == userId)
                                 .Select(ua => ua.Question.Questionnaire)
                                 .Distinct()
                                 .Include(q => q.Questions)
                                 .ThenInclude(q => q.Answers)
                                 .Include(q => q.Questions)
                                 .ThenInclude(q => q.UserAnswers.Where(ua => ua.UserId == userId))
                                 .ToListAsync();
        }

        public async Task<Questionnaire> GetAnsweredQuestionnaireDetailsAsync(int userId, int questionnaireId)
        {
            return await _context.Questionnaires
                                 .Where(q => q.Id == questionnaireId)
                                 .Include(q => q.Questions)
                                 .ThenInclude(q => q.Answers)
                                 .FirstOrDefaultAsync();
        }
    }
}
