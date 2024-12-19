using Microsoft.AspNetCore.Mvc;
using QuestionnaireSystem.Application.DTOs;
using QuestionnaireSystem.Application.Models;
using QuestionnaireSystem.Application.Services;
using QuestionnaireSystem.Application.Services.Interfaces;
using QuestionnaireSystem.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionnaireSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnairesController(IQuestionnaireService service) : ControllerBase
    {
        private readonly IQuestionnaireService _service = service;


        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuestionnaire([FromBody] QuestionnaireSubmission submission)
        {
            try
            {
                var result = await _service.SubmitQuestionnaireAsync(submission);
                return Ok("Questionnaire submitted!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(QuestionnaireDTO dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
    }
}
