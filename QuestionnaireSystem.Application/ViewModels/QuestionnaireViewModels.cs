namespace QuestionnaireSystem.Application.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
    }

    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class AnsweredQuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class AnsweredQuestionnaireDetailsViewModel
    {
        public string Title { get; set; }
        public List<AnsweredQuestionViewModel> Questions { get; set; }
    }

    public class AnsweredQuestionViewModel
    {
        public string Text { get; set; }
        public List<AnsweredAnswerViewModel> Answers { get; set; }
        public int SelectedAnswerId { get; set; }
    }

    public class AnsweredAnswerViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
