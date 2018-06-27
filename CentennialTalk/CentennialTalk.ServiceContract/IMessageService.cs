using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using System.Collections.Generic;

namespace CentennialTalk.ServiceContract
{
    public interface IMessageService
    {
        List<Message> GetChatMessages(string chatCode);

        bool SaveMessage(MessageDTO messageData);
    }
}