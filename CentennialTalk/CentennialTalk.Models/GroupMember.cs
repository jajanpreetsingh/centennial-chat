using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CentennialTalk.Models
{
    [Serializable]
    public class GroupMember
    {
        [Required]
        public Guid GroupMemberId { get; set; }

        [Required]
        [Key]
        [StringLength(255)]
        public string Username { get; set; }

        public string ConnectionId { get; set; }

        [Required]
        [StringLength(255)]
        public string ChatCode { get; set; }

        public IList<Message> Messages { get; set; }

        public bool IsConnected { get; set; }

        public GroupMember()
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();
        }

        public GroupMember(string username,string chatCode)
        {
            Messages = new List<Message>();

            GroupMemberId = new Guid();

            ChatCode = chatCode;

            Username = username;
        }

        public void SetConnectionId(string connId)
        {
            ConnectionId = connId;
        }
    }
}