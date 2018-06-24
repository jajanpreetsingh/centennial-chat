using CentennialTalk.Models;

namespace CentennialTalk.Service.Contracts
{
    public interface IChatService
    {
        Discussion CreateNewChat(string moderator, string title);

        Discussion GetChatByCode(string code);
    }
}