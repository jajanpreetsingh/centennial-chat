namespace CentennialTalk.Models.DTOModels
{
    public class QuestionDTO
    {
        public string content;
        public bool selectMultiple;
        public string[] options;
        public bool isPollingQuestion;
        public string chatCode;
    }
}