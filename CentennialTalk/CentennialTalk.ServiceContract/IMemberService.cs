using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using System.Collections.Generic;

namespace CentennialTalk.ServiceContract
{
    public interface IMemberService
    {
        bool UpdateConnectionStatus(ConnectionDetailDTO data);

        void DisconnectAllMembers();

        List<GroupMember> GetChatMembers(string chatCode);
    }
}