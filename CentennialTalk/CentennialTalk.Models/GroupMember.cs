using CentennialTalk.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CentennialTalk.Models
{
    [Serializable]
    public class GroupMember
    {
        [Key]
        [Required]
        public Guid GroupMemberId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        public string ConnectionId { get; set; }

        [Required]
        [StringLength(255)]
        public string ChatCode { get; set; }

        public IList<Message> Messages { get; set; }

        public bool IsConnected { get; set; }

        public bool IsModerator { get; set; }

        public DateTime JoiningTime { get; set; }

        public string IconPath { get; set; }

        public GroupMember()
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();
        }

        public GroupMember(NewChatDTO newChat, string chatCode)
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();

            ChatCode = chatCode;

            Username = newChat.moderator;

            IconPath = newChat.icon;

            IsModerator = true;
        }

        public GroupMember(JoinChatDTO joinChat)
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();

            ChatCode = joinChat.chatCode;

            Username = joinChat.username;

            IconPath = joinChat.icon;

            IsModerator = joinChat.isModerator;
        }

        public void SetConnectionId(string connId)
        {
            ConnectionId = connId;
        }

        public MemberDTO GetDTO()
        {
            return new MemberDTO()
            {
                memberId = GroupMemberId.ToString(),
                username = Username,
                iconName = IconPath,
                isConnected = IsConnected
            };
        }
    }
}