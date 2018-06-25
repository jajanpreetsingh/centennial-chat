using CentennialTalk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.PersistenceContract
{
    public interface IChatRepository
    {
        Discussion CreateNewChat(string moderator, string title);

        Discussion GetChatByCode(string code);
    }
}
