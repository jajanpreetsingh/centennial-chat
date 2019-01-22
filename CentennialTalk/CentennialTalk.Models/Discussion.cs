using CentennialTalk.Models.DTOModels;
using CentennialTalk.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CentennialTalk.Models
{
    [Serializable]
    public class Discussion
    {
        public Guid DiscussionId { get; set; }

        public string CreatorId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(8)]
        public string DiscussionCode { get; set; }

        public IList<GroupMember> Members { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsLinkOpen { get { return ActivationDate <= DateTime.Now && ExpirationDate >= DateTime.Now; } }

        public DateTime ActivationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public IList<PollingQuestion> Polls { get; set; }

        public IList<SubjectiveQuestion> Questions { get; set; }

        public Guid PublishedQuestionId { get; set; }

        public Discussion()
        {
            Members = new List<GroupMember>();

            Questions = new List<SubjectiveQuestion>();

            Polls = new List<PollingQuestion>();
        }

        public Discussion(NewChatDTO newChat)
        {
            Members = new List<GroupMember>();

            DiscussionCode = GenerateChatCode(newChat.moderator);

            CreatorId = newChat.creatorId;

            Questions = new List<SubjectiveQuestion>();

            if (newChat.openQuestions != null && newChat.openQuestions.Length > 0)
                newChat.openQuestions.ToList().ForEach(x =>
                {
                    x.chatCode = DiscussionCode;
                    Questions.Add(new SubjectiveQuestion(x));
                });

            Polls = new List<PollingQuestion>();

            if (newChat.pollQuestions != null && newChat.pollQuestions.Length > 0)
                newChat.pollQuestions.ToList().ForEach(x =>
                {
                    x.chatCode = DiscussionCode;
                    Polls.Add(new PollingQuestion(x));
                });

            GroupMember mod = new GroupMember(newChat, DiscussionCode);

            mod.IsModerator = true;

            Title = string.IsNullOrWhiteSpace(newChat.title)
                    ? string.Format("Discussion by {0}", mod.Username)
                    : newChat.title;

            Members.Add(mod);

            LastUpdated = DateTime.Now;

            CreatedDate = DateTime.Now;

            DateTime act;
            DateTime.TryParse(newChat.activationDate, out act);

            if (act != null && act > DateTime.MinValue && act < DateTime.MaxValue)
                ActivationDate = act;

            DateTime exp;
            DateTime.TryParse(newChat.expirationDate, out exp);

            if (exp != null && exp > DateTime.MinValue && exp < DateTime.MaxValue)
                ExpirationDate = exp;
        }

        private string GenerateChatCode(string moderator)
        {
            var chars = "ABCefghiDEFGHTUVWXY01234ZabcdjklmKLMNOnopqrstuvwxyz567IJPQRS89";
            var stringChars = new char[8];

            int seed = (moderator + DateTime.Now.ToString()).ToCharArray().ToList().Sum(x => x);

            Random random = new Random(seed);

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public DiscussionDTO GetResponseDTO()
        {
            DiscussionDTO dto = new DiscussionDTO();

            dto.chatCode = DiscussionCode;
            dto.title = Title;

            GroupMember mod = Members.FirstOrDefault(x => x.IsModerator);

            if (mod != null)
                dto.moderator = mod.Username;

            dto.activationDate = ActivationDate.ToString();
            dto.expirationDate = ExpirationDate.ToString();

            dto.pollQuestions = Polls.ToList().Select(x => x.GetDTO()).ToArray();
            dto.openQuestions = Questions.ToList().Select(x => x.GetDTO()).ToArray();

            dto.members = Members.ToList().Select(x => x.GetDTO()).ToArray();

            return dto;
        }
    }
}