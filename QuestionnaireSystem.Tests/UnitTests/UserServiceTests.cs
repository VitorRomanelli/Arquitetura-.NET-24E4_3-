using Moq;
using QuestionnaireSystem.Application.Services;
using QuestionnaireSystem.Data.Entities;
using QuestionnaireSystem.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuestionnaireSystem.Tests.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _service = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAnsweredQuestionnairesAsync_ShouldReturnQuestionnaires()
        {
            int userId = 1;
            var mockData = new List<Questionnaire> { new Questionnaire { Id = 1, Title = "Mock Questionnaire" } };
            _repositoryMock.Setup(repo => repo.GetAnsweredQuestionnairesAsync(userId)).ReturnsAsync(mockData);

            var result = await _service.GetAnsweredQuestionnairesAsync(userId);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Mock Questionnaire", result[0].Title);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenValidUserProvided()
        {
            var user = new User { Id = 1, Name = "John Doe", Email = "johndoe@example.com" };

            _repositoryMock.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync((User)null);
            _repositoryMock.Setup(repo => repo.CreateUserAsync(user)).ReturnsAsync(user);

            var result = await _service.CreateUserAsync(user);

            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("johndoe@example.com", result.Email);
            _repositoryMock.Verify(repo => repo.CreateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowArgumentException_WhenNameOrEmailIsMissing()
        {
            var invalidUser = new User { Id = 1, Name = "", Email = "" };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateUserAsync(invalidUser));
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowInvalidOperationException_WhenUserAlreadyExists()
        {
            var user = new User { Id = 1, Name = "John Doe", Email = "johndoe@example.com" };

            _repositoryMock.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateUserAsync(user));
        }

        [Fact]
        public async Task GetAnsweredQuestionnaireDetailsAsync_ShouldReturnQuestionnaire()
        {
            int userId = 1;
            int questionnaireId = 1;
            var mockQuestionnaire = new Questionnaire { Id = 1, Title = "Mock Questionnaire" };
            _repositoryMock.Setup(repo => repo.GetAnsweredQuestionnaireDetailsAsync(userId, questionnaireId)).ReturnsAsync(mockQuestionnaire);

            var result = await _service.GetAnsweredQuestionnaireDetailsAsync(userId, questionnaireId);

            Assert.NotNull(result);
            Assert.Equal("Mock Questionnaire", result.Title);
        }
    }
}
