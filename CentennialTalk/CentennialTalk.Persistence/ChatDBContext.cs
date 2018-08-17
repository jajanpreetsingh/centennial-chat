using CentennialTalk.Models;
using CentennialTalk.Models.QuestionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CentennialTalk.Persistence
{
    public class ChatDBContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Message> Messages { get; set; }

        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<MessageReaction> Reactions { get; set; }

        public DbSet<PollingQuestion> Polls { get; set; }

        public DbSet<SubjectiveQuestion> Questions { get; set; }

        //public DbSet<IdentityUser> ApplicationUsers { get; set; }

        public ChatDBContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }
    }
}