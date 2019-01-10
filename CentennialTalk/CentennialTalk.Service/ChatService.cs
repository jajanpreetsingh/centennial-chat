using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Service
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;
        private readonly ILogger<ChatService> logger;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger)
        {
            this.chatRepository = chatRepository;
            this.logger = logger;
        }

        public List<Discussion> GetChatsByCreatorId(string creatorId)
        {
            return chatRepository.GetChatsByCreatorId(creatorId);
        }

        public Discussion CreateNewChat(NewChatDTO newChat)
        {
            try
            {
                Discussion discussion = new Discussion(newChat);

                return chatRepository.CreateNewChat(discussion);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return null;
            }
        }

        public Discussion GetChatByCode(string code)
        {
            try
            {
                return chatRepository.GetChatByCode(code);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return null;
            }
        }

        public ResponseDTO JoinChat(JoinChatDTO joinChat)
        {
            try
            {
                Discussion chat = chatRepository.GetChatByCode(joinChat.chatCode, true);

                ResponseCode code = ResponseCode.ERROR;

                if (chat == null)
                    return new ResponseDTO(code, "Chat does not exist");

                if (!chat.IsLinkOpen)
                    return new ResponseDTO(code, "Link Closed for joining. Reach out to moderator");

                //if (chat.Members != null
                //    && chat
                //        .Members
                //        .Any(x => x.Username == joinChat.username && x.IsConnected))
                //    return new ResponseDTO(code,
                //        "A user with that name is already online on this chat");

                code = ResponseCode.OK;

                GroupMember member = null;

                if (chat.Members != null
                    && chat
                        .Members
                        .Any(x => x.Username == joinChat.username))
                {
                    member = chat.Members.FirstOrDefault(x => x.Username == joinChat.username);
                    member.IsConnected = true;

                    DiscussionDTO dt = chat.GetResponseDTO();
                    dt.username = member != null ? member.Username : joinChat.username;
                    dt.moderator = chat.Members.FirstOrDefault(x => x.IsModerator).Username;

                    return new ResponseDTO(code, dt);
                }

                member = new GroupMember(joinChat);

                chat.Members.Add(member);

                DiscussionDTO dto = chat.GetResponseDTO();
                dto.username = member != null ? member.Username : joinChat.username;
                dto.moderator = chat.Members.FirstOrDefault(x => x.IsModerator).Username;

                return new ResponseDTO(code, dto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return null;
            }
        }

        public List<Discussion> GetOpenChatGroups()
        {
            try
            {
                return chatRepository.GetOpenChatGroups();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return null;
            }
        }
    }
}