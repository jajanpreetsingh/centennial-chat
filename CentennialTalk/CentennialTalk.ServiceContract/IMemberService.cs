using CentennialTalk.Models.DTOModels;

namespace CentennialTalk.ServiceContract
{
    public interface IMemberService
    {
        bool UpdateConnectionStatus(ConnectionDetailDTO data);

        void DisconnectAllMembers();
    }
}