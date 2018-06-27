using CentennialTalk.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;

namespace CentennialTalk.Models
{
    [Serializable]
    public class Discussion
    {
        public Guid DiscussionId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(8)]
        public string DiscussionCode { get; set; }

        public IList<GroupMember> Members { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsLinkOpen { get; set; }

        public Discussion()
        {
            Members = new List<GroupMember>();

            DiscussionId = new Guid();

            IsLinkOpen = true;
        }

        public Discussion(NewChatDTO newChat)
        {
            Members = new List<GroupMember>();

            DiscussionId = new Guid();

            DiscussionCode = GenerateChatCode(newChat.moderator);

            GroupMember mod = new GroupMember(newChat.moderator, DiscussionCode, true);

            Title = string.IsNullOrWhiteSpace(newChat.title)
                    ? string.Format("Discussion by {0}", mod.Username)
                    : newChat.title;

            Members.Add(mod);

            LastUpdated = DateTime.Now;

            CreatedDate = DateTime.Now;

            IsLinkOpen = true;
        }

        private string GenerateChatCode(string moderator)
        {
            var chars = "ABCefghiDEFGHTUVWXY01234ZabcdjklmKLMNOnopqrstuvwxyz567IJPQRS89";
            var stringChars = new char[8];

            int seed = moderator.ToCharArray().ToList().Sum(x => (int)x);

            Random random = new Random(seed);

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public object GetResponseDTO()
        {
            ExpandoObject dto = new ExpandoObject();

            dto.TryAdd("id", DiscussionId);
            dto.TryAdd("chatCode", DiscussionCode);
            dto.TryAdd("title", Title);
            dto.TryAdd("moderator", Members.FirstOrDefault(x => x.IsModerator).Username);

            return dto;
        }
    }
}