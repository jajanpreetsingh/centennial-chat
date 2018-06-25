using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentennialTalk.Models
{
    [Serializable]
    public class Message
    {
        public Guid MessageId { get; set; }

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
            MessageId = new Guid();
        }

        public Message(string sender, string content, 
                    string chatCode, string replyMessageId = null)
        {
            MessageId = new Guid();
            Sender = sender;
            ChatCode = chatCode;
            Content = content;
            RepliedMessageId = replyMessageId;
        }
    }
}