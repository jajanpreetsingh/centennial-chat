namespace CentennialTalk.Models.DTOModels
{
    public class UserAnswerDTO
    {
        public string questionId;
        public string memberId;
        public string content;
        public bool selectMultiple;
        public string[] options;
        public bool isPollingQuestion;
        public string chatCode;
    }
}