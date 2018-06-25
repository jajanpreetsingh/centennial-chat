using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using System;

namespace CentennialTalk.Service
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }

        public bool UpdateConnectionStatus(ConnectionDetailDTO data)
        {
            GroupMember member = memberRepository.GetMemberByConnection(data);

            if (member == null)
                return false;

            member.ConnectionId = data.ConnectionId;
            member.IsConnected = data.IsConnected;

            return true;
        }
    }
}