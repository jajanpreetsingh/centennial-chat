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

        public List<Discussion> GetChatsByCreatorId(string creatorId)
        {
            return dbContext.Discussions.Include(x => x.Members)
                    .Include(x => x.Questions)
                    .Include(x => x.Polls)
                    .ThenInclude(y => y.Options).Where(x => x.CreatorId == creatorId).ToList();
        }

        public Discussion CreateNewChat(Discussion discussion)
        {
            dbContext.Discussions.Add(discussion);

            return discussion;
        }

        public Discussion GetChatByCode(string code, bool includeExtended = false)
        {
            if (!includeExtended)

                return dbContext.Discussions
                    .Where(x => x.DiscussionCode == code)
                    .FirstOrDefault();

            return dbContext.Discussions
                    .Where(x => x.DiscussionCode == code)
                    .Include(x => x.Members)
                    .Include(x => x.Questions)
                    .Include(x => x.Polls)
                    .ThenInclude(y => y.Options)
                    .FirstOrDefault();
        }
    }
}