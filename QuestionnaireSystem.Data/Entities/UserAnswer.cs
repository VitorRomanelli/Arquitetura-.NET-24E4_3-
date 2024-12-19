using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.Data.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public int SelectedAnswerId { get; set; }
        public Answer? SelectedAnswer { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
