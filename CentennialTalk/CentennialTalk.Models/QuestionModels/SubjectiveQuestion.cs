using CentennialTalk.Models.DTOModels;

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

        public SubjectiveQuestion(QuestionDTO dto) : base()
        {
            Type = QuestionType.SUBJECTIVE;

            Content = dto.content;
        }

        public QuestionDTO GetDTO()
        {
            return new QuestionDTO()
            {
                id = QuestionId.ToString(),
                chatCode = ChatCode,
                content = Content,
                isPollingQuestion = false,
                selectMultiple = false,
                isArchived = IsArchived,
                isPublished = IsPublished,
            };
        }
    }
}