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

        public string creatorId;

        public string icon;

        public QuestionDTO[] openQuestions;
        public QuestionDTO[] pollQuestions;
    }
}