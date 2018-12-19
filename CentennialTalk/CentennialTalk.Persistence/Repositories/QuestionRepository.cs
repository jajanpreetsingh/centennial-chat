using CentennialTalk.Models.QuestionModels;
using CentennialTalk.PersistenceContract;
using Microsoft.EntityFrameworkCore;
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
            Question ques = dbContext.Polls.Include(x => x.Options).FirstOrDefault(x => x.ChatCode == chatCode && x.Content == content);

            if (ques == null)
                ques = dbContext.Questions.FirstOrDefault(x => x.ChatCode == chatCode && x.Content == content);

            return ques;
        }

        public Question GetById(Guid id)
        {
            Question ques = dbContext.Polls.Include(x => x.Options).FirstOrDefault(x => x.QuestionId == id);

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

        public int SaveAnswers(List<UserAnswer> answers)
        {
            dbContext.Answers.AddRange(answers);

            return answers == null ? 0 : answers.Count;
        }

        public List<UserAnswer> GetAnswersByChat(string chatCode)
        {
            return dbContext.Answers.Where(x => x.ChatCode == chatCode).ToList();
        }
    }
}