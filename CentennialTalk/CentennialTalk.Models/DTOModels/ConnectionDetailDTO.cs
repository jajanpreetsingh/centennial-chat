using System;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class ConnectionDetailDTO
    {
        public string username;

        public string chatCode;

        public string connectionId;

        public bool isConnected;

        public ConnectionDetailDTO()
        { }
    }
}