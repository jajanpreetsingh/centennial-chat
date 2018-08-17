using System.ComponentModel.DataAnnotations;

namespace CentennialTalk.Models.QuestionModels
{
    public class QuestionOption
    {
        [Key]
        public int OptionId { get; set; }

        public string Text { get; set; }
    }
}