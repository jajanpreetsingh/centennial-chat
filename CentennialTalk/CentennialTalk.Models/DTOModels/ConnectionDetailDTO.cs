using System;
using System.Collections.Generic;
using System.Text;

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
