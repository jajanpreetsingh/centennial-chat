using CentennialTalk.ServiceContract;

namespace CentennialTalk.Service
{
    public class TestCases
    {
        private readonly IChatService ChatService;

        public TestCases(IChatService chatService)
        {
            ChatService = chatService;
        }

        public void AddTestData()
        {
        }
    }
}