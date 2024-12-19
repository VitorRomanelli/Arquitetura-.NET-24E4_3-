using Microsoft.EntityFrameworkCore;
using QuestionnaireSystem.Application.DTOs;
using QuestionnaireSystem.Application.Models;
using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;

namespace QuestionnaireSystem.Application.Services
{
    public class QuestionnaireService(
        IQuestionnaireRepository questionnaireRepository,
        IUserAnswerRepository userAnswerRepository) : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _repository = questionnaireRepository;
        private readonly IUserAnswerRepository _userAnswerRepository = userAnswerRepository;

        public async Task<QuestionnaireSubmissionResult> SubmitQuestionnaireAsync(QuestionnaireSubmission submission)
        {
            if (submission == null || submission.Answers.Count == 0)
                throw new ArgumentException("Invalid submission data.");

            var questionnaire = await _repository.GetQuestionnaireWithDetailsAsync(submission.QuestionnaireId) ?? throw new KeyNotFoundException("Questionnaire not found.");
            var userAnswers = new List<UserAnswer>();
            foreach (var answerSubmission in submission.Answers)
            {
                var question = questionnaire.Questions.FirstOrDefault(q => q.Id == answerSubmission.QuestionId) ?? throw new ArgumentException($"Invalid question ID: {answerSubmission.QuestionId}");
                var answer = question.Answers.FirstOrDefault(a => a.Id == answerSubmission.SelectedAnswerId) ?? throw new ArgumentException($"Invalid answer ID: {answerSubmission.SelectedAnswerId}");
                userAnswers.Add(new UserAnswer
                {
                    QuestionId = answerSubmission.QuestionId,
                    SelectedAnswerId = answerSubmission.SelectedAnswerId,
                    UserId = submission.UserId
                });
            }

            await _userAnswerRepository.SaveUserAnswerRangeAsync(userAnswers);

            var correctAnswersCount = userAnswers.Count(ua =>
                questionnaire.Questions
                    .First(q => q.Id == ua.QuestionId)
                    .Answers
                    .First(a => a.Id == ua.SelectedAnswerId)
                    .IsCorrect);

            var totalQuestions = questionnaire.Questions.Count;

            return new QuestionnaireSubmissionResult
            {
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswersCount,
                Score = (double)correctAnswersCount / totalQuestions * 100
            };
        }

        public async Task<List<QuestionnaireDTO>> GetAllAsync()
        {
            var questionnaires = await _repository.GetAllAsync();
            return questionnaires.Select(q => new QuestionnaireDTO
            {
                Id = q.Id,
                Title = q.Title,
                Questions = q.Questions.Select(qu => new QuestionDTO
                {
                    Id = qu.Id,
                    Text = qu.Text,
                    Answers = qu.Answers.Select(a => new AnswerDTO
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public async Task<QuestionnaireDTO> GetByIdAsync(int id)
        {
            var questionnaire = await _repository.GetByIdAsync(id);
            if (questionnaire == null) return null;

            return new QuestionnaireDTO
            {
                Id = questionnaire.Id,
                Title = questionnaire.Title,
                Questions = questionnaire.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new AnswerDTO
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };
        }

        public async Task AddAsync(QuestionnaireDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var questionnaire = new Questionnaire
            {
                Title = dto.Title,
                Questions = dto.Questions.Select(q =>
                {
                    var question = new Question
                    {
                        Text = q.Text,
                        Answers = q.Answers.Select(a => new Answer
                        {
                            Text = a.Text,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    };

                    return question;
                }).ToList()
            };

            await _repository.AddAsync(questionnaire);
        }
    }
}
