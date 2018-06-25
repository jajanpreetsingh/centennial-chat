using CentennialTalk.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.ServiceContract
{
    public interface IMemberService
    {
        bool UpdateConnectionStatus(ConnectionDetailDTO data);
    }
}
