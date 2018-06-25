using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentennialTalk.Persistence.Repositories
{
    public class MemberRepository: BaseRepository<GroupMember>, IMemberRepository
    {
        public MemberRepository(ChatDBContext context) : base(context)
        {
        }

        public GroupMember GetMemberByConnection(ConnectionDetailDTO data)
        {
            return dbContext.GroupMembers
                .FirstOrDefault(x => x.Username == data.Username 
                    && x.ChatCode == data.ChatCode);
        }
    }
}
