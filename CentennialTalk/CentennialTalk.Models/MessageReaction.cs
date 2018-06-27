using System;

namespace CentennialTalk.Models
{
    public class MessageReaction
    {
        public Guid MessageReactionId { get; set; }

        public string Sender { get; set; }

        public ReactType ReactType { get; set; }

        public MessageReaction()
        {
            MessageReactionId = new Guid();
        }

        public MessageReaction(string sender, ReactType reaction)
        {
            MessageReactionId = new Guid();

            Sender = sender;
            ReactType = reaction;
        }
    }

    public enum ReactType
    {
        LIKE = 1,
        DISLIKE = 2
    }
}