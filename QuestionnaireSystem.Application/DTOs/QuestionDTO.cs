using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Application.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<AnswerDTO> Answers { get; set; } = [];
    }
}
