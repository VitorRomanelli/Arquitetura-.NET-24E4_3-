using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Application.Models
{
    public class QuestionnaireSubmission
    {
        public int QuestionnaireId { get; set; }
        public int UserId { get; set; }
        public List<AnswerSubmission>? Answers { get; set; } = new();
    }

    public class QuestionnaireSubmissionResult
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Score { get; set; }
    }

    public class AnswerSubmission
    {
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
    }
}
