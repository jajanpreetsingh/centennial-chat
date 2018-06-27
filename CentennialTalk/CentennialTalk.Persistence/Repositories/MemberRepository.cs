using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Persistence.Repositories
{
    public class MemberRepository : BaseRepository<GroupMember>, IMemberRepository
    {
        public MemberRepository(ChatDBContext context) : base(context)
        {
        }

        public List<GroupMember> GetAll()
        {
            return dbContext.GroupMembers.ToList();
        }

        public GroupMember GetMemberByConnection(ConnectionDetailDTO data)
        {
            return dbContext.GroupMembers
                .FirstOrDefault(x => x.Username == data.username
                    && x.ChatCode == data.chatCode);
        }
    }
}