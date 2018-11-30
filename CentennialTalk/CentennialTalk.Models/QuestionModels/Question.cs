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

        public DateTime PublishDate { get; set; }

        public bool IsArchived { get; set; }

        public DateTime ArchiveDate { get; set; }

        public string ChatCode { get; set; }

        public TimeSpan DiscussionDuration
        {
            get
            {
                return ArchiveDate - PublishDate;
            }
        }

        public QuestionType Type { get; set; }

        public Question()
        {
        }
    }

    public enum QuestionType
    {
        POLLING = 1,
        SUBJECTIVE = 2
    }
}