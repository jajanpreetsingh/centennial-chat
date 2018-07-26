using System;

namespace CentennialTalk.Models.QuestionModels
{
    public class Question
    {
        public Guid QuestionId { get; set; }

        public string Content { get; set; }

        public QuestionType Type { get; set; }

        public Question()
        {
        }
    }

    public enum QuestionType
    {
        POLLING,
        SUBJECTIVE
    }
}