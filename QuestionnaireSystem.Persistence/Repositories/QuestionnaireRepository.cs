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
    public class QuestionnaireRepository(AppDbContext context) : IQuestionnaireRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Questionnaire>> GetAllAsync() =>
            await _context.Questionnaires.Include(q => q.Questions).ThenInclude(a => a.Answers).ToListAsync();

        public async Task<Questionnaire> GetByIdAsync(int id) =>
            await _context.Questionnaires.Include(q => q.Questions).ThenInclude(a => a.Answers)
                                         .FirstOrDefaultAsync(q => q.Id == id);

        public async Task AddAsync(Questionnaire questionnaire)
        {
            await _context.Questionnaires.AddAsync(questionnaire);
            await _context.SaveChangesAsync();
        }

        public async Task<Questionnaire?> GetQuestionnaireWithDetailsAsync(int questionnaireId)
        {
            return await _context.Questionnaires
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionnaireId);
        }
    }
}
