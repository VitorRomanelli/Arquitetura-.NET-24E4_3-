using Microsoft.AspNetCore.Mvc;
using QuestionnaireSystem.Application.Services;
using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Application.ViewModels;
using QuestionnaireSystem.Data.Entities;

namespace QuestionnaireSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _service.GetUserByIdAsync(userId);
            if (user == null) return NotFound(new { Message = "User not found." });
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var createdUser = await _service.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        [HttpGet("{userId}/questionnaires")]
        public async Task<IActionResult> GetAnsweredQuestionnaires(int userId)
        {
            var questionnaires = await _service.GetAnsweredQuestionnairesAsync(userId);
            return Ok(questionnaires);
        }

        [HttpGet("{userId}/questionnaires/{questionnaireId}")]
        public async Task<IActionResult> GetAnsweredQuestionnaireDetails(int userId, int questionnaireId)
        {
            var questionnaire = await _service.GetAnsweredQuestionnaireDetailsAsync(userId, questionnaireId);
            if (questionnaire == null) return NotFound(new { Message = "Questionnaire not found." });
            return Ok(questionnaire);
        }
    }
}
