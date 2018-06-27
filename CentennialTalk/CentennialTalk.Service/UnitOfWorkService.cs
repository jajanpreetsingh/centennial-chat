using CentennialTalk.Persistence;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System;

namespace CentennialTalk.Service
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly ChatDBContext chatDBContext;
        private readonly ILogger<UnitOfWorkService> logger;

        public UnitOfWorkService(ChatDBContext chatDBContext, ILogger<UnitOfWorkService> logger)
        {
            this.chatDBContext = chatDBContext;
            this.logger = logger;
        }

        public bool SaveChanges()
        {
            try
            {
                int result = chatDBContext.SaveChanges();

                return result >= 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return false;
            }
        }
    }
}