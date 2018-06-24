using CentennialTalk.Models;
using CentennialTalk.Persistence.Contracts;
using System.Linq;

namespace CentennialTalk.Persistence.Repositories
{
    public class ChatRepository : BaseRepository<Discussion>, IChatRepository
    {
        public ChatRepository(ChatDBContext context) : base(context)
        {
        }

        public Discussion CreateNewChat(string moderator, string title)
        {
            try
            {
                Discussion discussion = new Discussion(moderator, title);

                dbContext.Discussions.Add(discussion);

                dbContext.SaveChanges();

                return discussion;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public Discussion GetChatByCode(string code)
        {
            try
            {
                return dbContext.Discussions.FirstOrDefault(x => x.DiscussionCode == code);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}