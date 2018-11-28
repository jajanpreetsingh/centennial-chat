using CentennialTalk.Models.QuestionModels;
using CentennialTalk.PersistenceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Persistence.Repositories
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ChatDBContext context) : base(context)
        {
        }

        public Question GetByChatCodeNContent(string chatCode, string content)
        {
            Question ques = dbContext.Polls.FirstOrDefault(x => x.ChatCode == chatCode && x.Content == content);

            if (ques == null)
                ques = dbContext.Questions.FirstOrDefault(x => x.ChatCode == chatCode && x.Content == content);

            return ques;
        }

        public Question GetById(Guid id)
        {
            Question ques = dbContext.Polls.FirstOrDefault(x => x.QuestionId == id);

            if (ques == null)
                ques = dbContext.Questions.FirstOrDefault(x => x.QuestionId == id);

            return ques;
        }

        public List<PollingQuestion> GetChatPollingQuestions(string chatCode)
        {
            return dbContext.Polls.Where(x => x.ChatCode == chatCode).ToList();
        }

        public List<SubjectiveQuestion> GetChatSubjectiveQuestions(string chatCode)
        {
            return dbContext.Questions.Where(x => x.ChatCode == chatCode).ToList();
        }
    }
}