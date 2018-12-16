using System;

namespace CentennialTalk.Models.QuestionModels
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public Guid QuestionId { get; set; }

        public Guid MemberId { get; set; }

        public string ChatCode { get; set; }

        public string Content { get; set; }

        public int OptionId { get; set; }
    }
}