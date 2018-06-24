using CentennialTalk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.Persistence.Contracts
{
    public interface IChatRepository
    {
        Discussion CreateNewChat(string moderator, string title);

        Discussion GetChatByCode(string code);
    }
}
