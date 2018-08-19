using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using System.Collections.Generic;

namespace CentennialTalk.PersistenceContract
{
    public interface IMemberRepository
    {
        GroupMember GetMemberByConnection(ConnectionDetailDTO data);

        List<GroupMember> GetAll();

        List<GroupMember> GetChatMembers(string chatCode);
    }
}