using CentennialTalk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.PersistenceContract
{
    public interface IChatRepository
    {
        Discussion CreateNewChat(Discussion discussion);

        Discussion GetChatByCode(string code);

        List<Discussion> GetOpenChatGroups();
    }
}
