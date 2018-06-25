using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.Models.DTOModels
{
    [Serializable]
    public class MessageDTO
    {
        public string content;

        public string sender;

        public string chatCode;
    }
}
