using CentennialTalk.Models.DTOModels;
using System.Collections.Generic;
using System.Linq;

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

        public PollingQuestion(QuestionDTO dto) : base()
        {
            Type = QuestionType.POLLING;

            Options = new List<QuestionOption>();

            SelectMultiple = dto.selectMultiple;

            Content = dto.content;

            if (dto.options != null && dto.options.Length > 0)
                dto.options.ToList().ForEach(x => Options.Add(new QuestionOption() { Text = x }));
        }

        public PollingQuestion(string content) : base()
        {
            Content = content;
            Type = QuestionType.POLLING;
        }
    }
}