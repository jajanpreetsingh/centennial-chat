using System;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class MessageDTO
    {
        //public Guid messageId;

        public string content;

        public string sender;

        public string chatCode;

        //public Guid replyId;

        //public DateTime sentDate;
    }
}