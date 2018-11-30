namespace CentennialTalk.Models.DTOModels
{
    public class DiscussionDTO
    {
        public string chatCode;
        public string moderator;
        public string username;
        public string title;
        public string connectionId;
        public string activationDate;
        public string expirationDate;
        public QuestionDTO[] openQuestions;
        public QuestionDTO[] pollQuestions;
        public MemberDTO[] members;
    }
}