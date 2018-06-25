using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CentennialTalk.Models
{
    [Serializable]
    public class Discussion
    {
        public Guid DiscussionId { get; set; }

        [Required]
        [StringLength(255)]
        public GroupMember Moderator { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(8)]
        public string DiscussionCode { get; set; }

        public IList<GroupMember> Members { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsLinkOpen { get; set; }

        public Discussion()
        {
            Members = new List<GroupMember>();

            DiscussionId = new Guid();
        }

        public Discussion(string moderator, string title)
        {
            Members = new List<GroupMember>();

            DiscussionId = new Guid();

            Title = string.IsNullOrWhiteSpace(title)
                    ? string.Format("Discussion by {0}", Moderator)
                    : title;

            DiscussionCode = GenerateChatCode();

            Moderator = new GroupMember(moderator, DiscussionCode);

            Members.Add(Moderator);

            LastUpdated = DateTime.Now;
        }

        private string GenerateChatCode()
        {
            var chars = "ABCefghiDEFGHTUVWXY01234ZabcdjklmKLMNOnopqrstuvwxyz567IJPQRS89";
            var stringChars = new char[8];

            int seed = Moderator.Username.ToCharArray().ToList().Sum(x => (int)x);

            Random random = new Random(seed);

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}