using CentennialTalk.Models;
using CentennialTalk.Persistence.Contracts;
using CentennialTalk.Service.Contracts;

namespace CentennialTalk.Service.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public Discussion CreateNewChat(string moderator, string title)
        {
            return chatRepository.CreateNewChat(moderator, title);
        }

        public Discussion GetChatByCode(string code)
        {
            return chatRepository.GetChatByCode(code);
        }
    }
}