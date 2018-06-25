using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class ConnectionDetailDTO
    {
        public string Username;

        public string ChatCode;

        public string ConnectionId;

        public bool IsConnected;

        public ConnectionDetailDTO()
        { }
    }
}
