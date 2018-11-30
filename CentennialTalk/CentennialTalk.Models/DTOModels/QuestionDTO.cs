namespace CentennialTalk.Models.DTOModels
{
    public class QuestionDTO
    {
        public string id;
        public string content;
        public bool selectMultiple;
        public string[] options;
        public bool isPollingQuestion;
        public string chatCode;
        public bool isPublished;
        public bool isArchived;

        public string publishDate;
        public string archiveDate;
    }
}