using QuestionnaireSystem.Application.DTOs;
using QuestionnaireSystem.Application.Models;

namespace QuestionnaireSystem.Application.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        Task<QuestionnaireSubmissionResult> SubmitQuestionnaireAsync(QuestionnaireSubmission submission);
        Task AddAsync(QuestionnaireDTO dto);
        Task<List<QuestionnaireDTO>> GetAllAsync();
        Task<QuestionnaireDTO> GetByIdAsync(int id);
    }
}