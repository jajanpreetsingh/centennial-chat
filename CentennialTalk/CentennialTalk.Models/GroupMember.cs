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

        public GroupMember()
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();
        }

        public GroupMember(string username, string chatCode, bool isModerator = false)
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();

            ChatCode = chatCode;

            Username = username;

            IsModerator = isModerator;
        }

        public GroupMember(JoinChatDTO joinChat)
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();

            ChatCode = joinChat.chatCode;

            Username = joinChat.username;

            IsModerator = joinChat.isModerator;
        }

        public void SetConnectionId(string connId)
        {
            ConnectionId = connId;
        }
    }
}