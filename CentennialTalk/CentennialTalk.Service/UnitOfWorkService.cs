using CentennialTalk.Persistence;
using CentennialTalk.ServiceContract;

namespace CentennialTalk.Service
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly ChatDBContext chatDBContext;

        public UnitOfWorkService(ChatDBContext chatDBContext)
        {
            this.chatDBContext = chatDBContext;
        }

        public bool SaveChanges()
        {
            int result = chatDBContext.SaveChanges();

            return result >= 0;
        }
    }
}