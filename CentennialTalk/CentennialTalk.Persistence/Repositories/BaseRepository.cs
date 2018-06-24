using CentennialTalk.Persistence.Contracts;

namespace CentennialTalk.Persistence.Repositories
{
    public class BaseRepository<T> where T : class
    {
        public ChatDBContext dbContext;

        public BaseRepository(ChatDBContext chatDBContext)
        {
            dbContext = chatDBContext;
        }
    }
}