using CentennialTalk.Models;
using CentennialTalk.PersistenceContract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Persistence.Repositories
{
    public class ChatRepository : BaseRepository<Discussion>, IChatRepository
    {
        public ChatRepository(ChatDBContext context) : base(context)
        {
        }

        public List<Discussion> GetOpenChatGroups()
        {
            return dbContext.Discussions.Where(x => x.IsLinkOpen).ToList();
        }

        public Discussion CreateNewChat(Discussion discussion)
        {
            dbContext.Discussions.Add(discussion);

            return discussion;
        }

        public Discussion GetChatByCode(string code)
        {
            return dbContext.Discussions
                .Where(x => x.DiscussionCode == code)
                .Include(x => x.Members)
                .FirstOrDefault();
        }
    }
}