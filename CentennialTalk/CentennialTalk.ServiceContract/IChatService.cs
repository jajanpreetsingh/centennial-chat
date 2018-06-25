using CentennialTalk.Models;

namespace CentennialTalk.ServiceContract
{
    public interface IChatService
    {
        Discussion CreateNewChat(string moderator, string title);

        Discussion GetChatByCode(string code);
    }
}