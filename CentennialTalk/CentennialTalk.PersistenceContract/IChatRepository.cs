using CentennialTalk.Models;
using System.Collections.Generic;

namespace CentennialTalk.PersistenceContract
{
    public interface IChatRepository
    {
        Discussion CreateNewChat(Discussion discussion);

        Discussion GetChatByCode(string code, bool includeMembers = false);

        List<Discussion> GetOpenChatGroups();
    }
}