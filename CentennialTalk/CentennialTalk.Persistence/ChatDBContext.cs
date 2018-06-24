using CentennialTalk.Models;
using CentennialTalk.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CentennialTalk.Persistence
{
    public class ChatDBContext : DbContext, IChatDBContext
    {
        public DbSet<Message> Messages { get; set; }

        public DbSet<Discussion> Discussions { get; set; }

        public ChatDBContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }
    }
}