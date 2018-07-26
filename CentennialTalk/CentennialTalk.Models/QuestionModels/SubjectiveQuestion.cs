namespace CentennialTalk.Models.QuestionModels
{
    public class SubjectiveQuestion : Question
    {
        public SubjectiveQuestion() : base()
        {
            Type = QuestionType.SUBJECTIVE;
        }

        public SubjectiveQuestion(string content) : base()
        {
            Content = content;
            Type = QuestionType.POLLING;
        }
    }
}