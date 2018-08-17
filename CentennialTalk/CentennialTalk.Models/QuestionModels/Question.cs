using System;
using System.ComponentModel.DataAnnotations;

namespace CentennialTalk.Models.QuestionModels
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }

        public string Content { get; set; }

        public bool IsPublished { get; set; }

        public QuestionType Type { get; set; }

        public Question()
        {
            IsPublished = false;
        }
    }

    public enum QuestionType
    {
        POLLING,
        SUBJECTIVE
    }
}