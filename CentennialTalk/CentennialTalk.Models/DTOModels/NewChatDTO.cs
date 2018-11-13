using System;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class NewChatDTO
    {
        public string moderator;
        public string title;
        public string activationDate;
        public string expirationDate;

        public QuestionDTO[] openQuestions;
        public QuestionDTO[] pollQuestions;
    }
}