using CentennialTalk.Models;
using System.Collections.Generic;

namespace CentennialTalk.PersistenceContract
{
    public interface IChatRepository
    {
        Discussion CreateNewChat(Discussion discussion);

        Discussion GetChatByCode(string code, bool includeExtended = false);

        List<Discussion> GetOpenChatGroups();

        List<Discussion> GetChatsByCreatorId(string creatorId);
    }
}