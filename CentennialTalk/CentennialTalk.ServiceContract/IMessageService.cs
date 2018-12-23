using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using System;
using System.Collections.Generic;

namespace CentennialTalk.ServiceContract
{
    public interface IMessageService
    {
        List<Message> GetChatMessages(string chatCode);

        bool SaveMessage(MessageDTO messageData);

        Message SaveReaction(ReactionDTO reactDto);

        Message GetMessageById(Guid guid);
    }
}