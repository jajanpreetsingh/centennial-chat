using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class JoinChatDTO
    {
        public string username;

        public string chatCode;

        public bool isModerator;
    }
}
