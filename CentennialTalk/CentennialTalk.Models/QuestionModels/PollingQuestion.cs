using System.Collections.Generic;

namespace CentennialTalk.Models.QuestionModels
{
    public class PollingQuestion : Question
    {
        public bool SelectMultiple { get; set; }

        public IList<QuestionOption> Options { get; set; }

        public PollingQuestion() : base()
        {
            Type = QuestionType.POLLING;

            Options = new List<QuestionOption>();
        }

        public PollingQuestion(string content) : base()
        {
            Content = content;
            Type = QuestionType.POLLING;
        }
    }
}