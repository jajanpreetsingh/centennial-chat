using CentennialTalk.Models.DTOModels;
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
        }

        public MessageReaction(string sender, ReactType reaction)
        {
            Sender = sender;
            ReactType = reaction;
        }

        public ReactionDTO GetResponseDTO()
        {
            ReactionDTO dto = new ReactionDTO();

            dto.member = Sender;
            dto.reaction = ReactType == ReactType.LIKE
                           ? 1
                           : ReactType == ReactType.DISLIKE
                                            ? -1 : 0;

            return dto;
        }
    }

    public enum ReactType
    {
        LIKE = 1,
        DISLIKE = 2
    }
}