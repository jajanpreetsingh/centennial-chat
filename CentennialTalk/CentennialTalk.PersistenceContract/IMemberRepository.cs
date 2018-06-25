using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentennialTalk.PersistenceContract
{
    public interface IMemberRepository
    {
        GroupMember GetMemberByConnection(ConnectionDetailDTO data);
    }
}
