using CentennialTalk.Models;
using Microsoft.EntityFrameworkCore;

namespace CentennialTalk.Persistence
{
    public class ChatDBContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; } 

        public DbSet<MessageReaction> Reactions { get; set; }

        public ChatDBContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }
    }
}