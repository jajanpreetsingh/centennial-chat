using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CentennialTalk.Models
{
    public class Discussion
    {
        public Guid DiscussionId { get; set; }

        [Required]
        [StringLength(255)]
        public string Moderator { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(8)]
        public string DiscussionCode { get; set; }

        [NotMapped]
        public IList<string> Members { get; set; }

        public DateTime LastUpdated { get; set; }

        public Discussion()
        { }

        public Discussion(string moderator, string title)
        {
            Members = new List<string>();

            Title = string.IsNullOrWhiteSpace(title)
                    ? string.Format("Discussion by {0}", Moderator)
                    : title;

            Moderator = moderator;

            DiscussionCode = GenerateChatCode();

            DiscussionId = new Guid();

            Members.Add(Moderator);

            LastUpdated = DateTime.Now;
        }

        private string GenerateChatCode()
        {
            var chars = "ABCefghiDEFGHTUVWXY01234ZabcdjklmKLMNOnopqrstuvwxyz567IJPQRS89";
            var stringChars = new char[8];

            int seed = Moderator.ToCharArray().ToList().Sum(x => (int)x);

            Random random = new Random(seed);

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}