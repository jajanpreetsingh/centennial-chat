using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentennialTalk.Models
{
    public class Message
    {
        public string MessageId { get; set; }

        [Required]
        [StringLength(255)]
        public string Content { get; set; }

        [Required]
        [StringLength(255)]
        public string Sender { get; set; }

        [Required]
        [StringLength(8)]
        public string ChatCode { get; set; }

        public string RepliedMessageId { get; set; }

        [NotMapped]
        public bool IsAReply
        {
            get
            {
                return !string.IsNullOrWhiteSpace(RepliedMessageId);
            }
        }

        public Message()
        {
        }

        public Message(string sender, string content, 
                    string chatCode, string replyMessageId = null)
        {
            Sender = sender;
            ChatCode = chatCode;
            Content = content;
            RepliedMessageId = replyMessageId;
        }
    }
}