using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Persistence.Repositories
{
    public class UserAnswerRepository : IUserAnswerRepository
    {
        private readonly AppDbContext _context;

        public UserAnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveUserAnswerAsync(UserAnswer userAnswer)
        {
            await _context.AddAsync(userAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task SaveUserAnswerRangeAsync(List<UserAnswer> userAnswers)
        {
            _context.UserAnswers.AddRange(userAnswers);
            await _context.SaveChangesAsync();
        }

    }
}
