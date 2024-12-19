using Moq;
using QuestionnaireSystem.Application.DTOs;
using QuestionnaireSystem.Application.Models;
using QuestionnaireSystem.Application.Services;
using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;
using Xunit;

namespace QuestionnaireSystem.Tests.UnitTests
{
    public class QuestionnaireServiceTests
    {
        private readonly Mock<IQuestionnaireRepository> _repositoryMock;
        private readonly Mock<IUserAnswerRepository> _userAnswerRepositoryMock;
        private readonly QuestionnaireService _service;

        public QuestionnaireServiceTests()
        {
            _repositoryMock = new Mock<IQuestionnaireRepository>();
            _userAnswerRepositoryMock = new Mock<IUserAnswerRepository>();
            _service = new QuestionnaireService(_repositoryMock.Object, _userAnswerRepositoryMock.Object);
        }

        #region SubmitQuestionnaire Tests

        [Fact]
        public async Task SubmitQuestionnaireAsync_ShouldReturnCorrectResults_WhenAnswersAreCorrect()
        {
            var questionnaireId = 1;
            var userId = 1;
            var submission = new QuestionnaireSubmission
            {
                QuestionnaireId = questionnaireId,
                UserId = userId,
                Answers =
            [
                new AnswerSubmission { QuestionId = 1, SelectedAnswerId = 1 },
                new AnswerSubmission { QuestionId = 2, SelectedAnswerId = 3 }
            ]
            };

            var questionnaire = new Questionnaire
            {
                Id = questionnaireId,
                Questions =
            [
                new Question
                {
                    Id = 1,
                    Answers =
                    [
                        new Answer { Id = 1, IsCorrect = true },
                        new Answer { Id = 2, IsCorrect = false }
                    ]
                },
                new Question
                {
                    Id = 2,
                    Answers =
                    [
                        new Answer { Id = 3, IsCorrect = true },
                        new Answer { Id = 4, IsCorrect = false }
                    ]
                }
            ]
            };

            _repositoryMock
                .Setup(repo => repo.GetQuestionnaireWithDetailsAsync(questionnaireId))
                .ReturnsAsync(questionnaire);

            _userAnswerRepositoryMock
                .Setup(repo => repo.SaveUserAnswerRangeAsync(It.IsAny<List<UserAnswer>>()))
                .Returns(Task.CompletedTask);

            var result = await _service.SubmitQuestionnaireAsync(submission);

            Assert.NotNull(result);
            var resultObj = Assert.IsType<QuestionnaireSubmissionResult>(result);
            Assert.Equal(2, resultObj.TotalQuestions);
            Assert.Equal(2, resultObj.CorrectAnswers);
            Assert.Equal(100.0, resultObj.Score);
        }

        [Fact]
        public async Task SubmitQuestionnaireAsync_ShouldThrowArgumentException_WhenSubmissionIsInvalid()
        {
            var submission = new QuestionnaireSubmission();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.SubmitQuestionnaireAsync(submission));
        }

        [Fact]
        public async Task SubmitQuestionnaireAsync_ShouldSaveUserAnswers()
        {
            var questionnaireId = 1;
            var userId = 1;
            var submission = new QuestionnaireSubmission
            {
                QuestionnaireId = questionnaireId,
                UserId = userId,
                Answers =
                [
                    new AnswerSubmission { QuestionId = 1, SelectedAnswerId = 1 },
                    new AnswerSubmission { QuestionId = 2, SelectedAnswerId = 3 }
                ]
            };

            var questionnaire = new Questionnaire
            {
                Id = questionnaireId,
                Questions =
            [
                new Question
                {
                    Id = 1,
                    Answers =
                    [
                        new Answer { Id = 1, IsCorrect = true },
                        new Answer { Id = 2, IsCorrect = false }
                    ]
                },
                new Question
                {
                    Id = 2,
                    Answers =
                    [
                        new Answer { Id = 3, IsCorrect = true },
                        new Answer { Id = 4, IsCorrect = false }
                    ]
                }
            ]
            };

            _repositoryMock
                .Setup(repo => repo.GetQuestionnaireWithDetailsAsync(questionnaireId))
                .ReturnsAsync(questionnaire);

            _userAnswerRepositoryMock
                .Setup(repo => repo.SaveUserAnswerRangeAsync(It.IsAny<List<UserAnswer>>()))
                .Returns(Task.CompletedTask);

            await _service.SubmitQuestionnaireAsync(submission);

            _userAnswerRepositoryMock.Verify(repo => repo.SaveUserAnswerRangeAsync(It.Is<List<UserAnswer>>(answers =>
                answers.Count == 2 &&
                answers.Any(ua => ua.QuestionId == 1 && ua.SelectedAnswerId == 1 && ua.UserId == userId) &&
                answers.Any(ua => ua.QuestionId == 2 && ua.SelectedAnswerId == 3 && ua.UserId == userId)
            )), Times.Once);
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfQuestionnaireDTO_WhenQuestionnairesExist()
        {
            var questionnaires = new List<Questionnaire>
        {
            new Questionnaire
            {
                Id = 1,
                Title = "Test Questionnaire",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Id = 1,
                        Text = "Question 1",
                        Answers = new List<Answer>
                        {
                            new Answer { Id = 1, Text = "Answer 1", IsCorrect = true },
                            new Answer { Id = 2, Text = "Answer 2", IsCorrect = false }
                        }
                    }
                }
            }
        };

            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(questionnaires);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test Questionnaire", result.First().Title);
            Assert.Single(result.First().Questions);
            Assert.Equal("Question 1", result.First().Questions.First().Text);
            _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoQuestionnairesExist()
        {
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Questionnaire>());

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuestionnaireDTO_WhenQuestionnaireExists()
        {
            var questionnaire = new Questionnaire
            {
                Id = 1,
                Title = "Test Questionnaire",
                Questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Text = "Question 1",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, Text = "Answer 1", IsCorrect = true },
                        new Answer { Id = 2, Text = "Answer 2", IsCorrect = false }
                    }
                }
            }
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(questionnaire);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Test Questionnaire", result.Title);
            Assert.Single(result.Questions);
            Assert.Equal("Question 1", result.Questions.First().Text);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenQuestionnaireDoesNotExist()
        {
            _repositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Questionnaire)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        #endregion

        #region AddAsync Tests

        [Fact]
        public async Task AddAsync_ShouldAddQuestionnaire_WhenValidDTOProvided()
        {
            var questionnaireDTO = new QuestionnaireDTO
            {
                Title = "Test Questionnaire",
                Questions = new List<QuestionDTO>
            {
                new QuestionDTO
                {
                    Text = "Question 1",
                    Answers = new List<AnswerDTO>
                    {
                        new AnswerDTO { Text = "Answer 1", IsCorrect = true },
                        new AnswerDTO { Text = "Answer 2", IsCorrect = false }
                    }
                }
            }
            };

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Questionnaire>())).Returns(Task.CompletedTask);

            await _service.AddAsync(questionnaireDTO);

            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Questionnaire>(
                q => q.Title == "Test Questionnaire" &&
                     q.Questions.Count == 1 &&
                     q.Questions.First().Answers.Count == 2
            )), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenDTOIsNull()
        {
            QuestionnaireDTO questionnaireDTO = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddAsync(questionnaireDTO));
        }

        #endregion
    }
}
