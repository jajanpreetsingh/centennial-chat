using CentennialTalk.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public Guid RepliedMessageId { get; set; }

        public DateTime SentDate { get; set; }

        public IList<MessageReaction> Reactions { get; set; }

        [NotMapped]
        public bool IsAReply
        {
            get
            {
                return RepliedMessageId != Guid.Empty
                    && RepliedMessageId != MessageId;
            }
        }

        public Message()
        {
            Reactions = new List<MessageReaction>();
        }

        public Message(MessageDTO messageData)
        {
            MessageId = messageData.messageId;//new Guid();

            Reactions = new List<MessageReaction>();

            Sender = messageData.sender;
            ChatCode = messageData.chatCode;
            Content = messageData.content;

            SentDate = messageData.sentDate;
        }

        public void SetReplyId(Guid guid)
        {
            RepliedMessageId = guid;
        }

        public MessageDTO GetResponseDTO()
        {
            MessageDTO dto = new MessageDTO();

            dto.messageId = MessageId;
            dto.chatCode = ChatCode;
            dto.content = Content;
            dto.sender = Sender;
            dto.replyId = RepliedMessageId;
            dto.sentDate = SentDate;

            if (Reactions != null && Reactions.Count > 0)
            {
                dto.reactions = Reactions.Select(x => x.GetResponseDTO()).ToArray();

                for (int i = 0; i < dto.reactions.Length; i++)
                {
                    dto.reactions[i].messageId = dto.messageId.ToString();
                }
            }
            return dto;
        }
    }
}