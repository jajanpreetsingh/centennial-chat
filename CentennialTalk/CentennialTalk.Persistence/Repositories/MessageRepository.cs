using CentennialTalk.Models;
using CentennialTalk.PersistenceContract;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Persistence.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDBContext context) : base(context)
        {
        }

        public List<Message> GetChatMessages(string chatCode)
        {
            return dbContext.Messages.Where(x => x.ChatCode == chatCode).ToList();
        }

        public bool SaveMessage(Message message)
        {
            GroupMember member = dbContext
                                .GroupMembers
                                .FirstOrDefault(x => x.Username == message.Sender
                                                  && x.ChatCode == message.ChatCode);

            if (member == null)
                return false;

            member.Messages.Add(message);

            return true;
        }
    }
}