using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CentennialTalk.Service
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;
        private readonly ILogger<MemberService> logger;

        public MemberService(IMemberRepository memberRepository, ILogger<MemberService> logger)
        {
            this.memberRepository = memberRepository;
            this.logger = logger;
        }

        public bool UpdateConnectionStatus(ConnectionDetailDTO data)
        {
            try
            {
                GroupMember member = memberRepository.GetMemberByConnection(data);

                if (member == null)
                    return false;

                member.ConnectionId = data.connectionId;
                member.IsConnected = data.isConnected;

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return false;
            }
        }

        public void DisconnectAllMembers()
        {
            try
            {
                List<GroupMember> members = memberRepository.GetAll();

                members.ForEach(x => x.IsConnected = false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}