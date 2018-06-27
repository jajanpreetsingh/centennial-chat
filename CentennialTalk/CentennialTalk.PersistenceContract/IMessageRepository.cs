using CentennialTalk.Models;
using System.Collections.Generic;

namespace CentennialTalk.PersistenceContract
{
    public interface IMessageRepository
    {
        List<Message> GetChatMessages(string chatCode);

        bool SaveMessage(Message message);
    }
}