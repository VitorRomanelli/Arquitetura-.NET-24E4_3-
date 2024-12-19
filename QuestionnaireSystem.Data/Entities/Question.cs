using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int QuestionnaireId { get; set; }
        public Questionnaire? Questionnaire { get; set; }
        public List<Answer> Answers { get; set; } = [];
        public List<UserAnswer> UserAnswers { get; set; } = [];
    }
}
