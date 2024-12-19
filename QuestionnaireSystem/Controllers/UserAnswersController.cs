using Microsoft.AspNetCore.Mvc;
using QuestionnaireSystem.Application.Services.Interfaces;

namespace QuestionnaireSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswersController(IUserAnswerService service) : ControllerBase
    {
        private readonly IUserAnswerService _service = service;

        [HttpPost]
        public async Task<IActionResult> SaveUserAnswer(int questionId, int selectedAnswerId, int userId)
        {
            try
            {
                await _service.SaveUserAnswerAsync(questionId, selectedAnswerId, userId);
                return Ok(new { Message = "Answer saved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
