using System;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class JoinChatDTO
    {
        public string username;

        public string icon;

        public string chatCode;

        public bool isModerator;
    }
}