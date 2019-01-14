using CentennialTalk.Models.DTOModels;
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

        public List<UserAnswer> GetPreviousAnswers(UserAnswerDTO answer)
        {
            return dbContext.Answers.Where(x => x.MemberId == Guid.Parse(answer.memberId) && x.ChatCode == answer.chatCode).ToList();
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

        public PollingQuestion GetPollById(Guid id)
        {
            PollingQuestion ques = dbContext.Polls.Include(x => x.Options).FirstOrDefault(x => x.QuestionId == id);

            return ques;
        }

        public SubjectiveQuestion GetOpenQuesById(Guid id)
        {
            SubjectiveQuestion ques = dbContext.Questions.FirstOrDefault(x => x.QuestionId == id);

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

        public List<QuestionTrainingModel> GetAllSubjectiveAnswers()
        {
            List<QuestionTrainingModel> trainData = new List<QuestionTrainingModel>();

            List<SubjectiveQuestion> questions = dbContext.Questions.ToList();

            if (questions == null)
                return trainData;

            List<UserAnswer> answers = dbContext.Answers.Where(x => questions.Any(y => y.QuestionId == x.QuestionId)).ToList();

            answers.ForEach(x => 
            {
                QuestionTrainingModel q = new QuestionTrainingModel();

                q.Answer = x.Content;
                q.Question = questions.FirstOrDefault(y => y.QuestionId == x.QuestionId).Content;

                trainData.Add(q);
            });

            return trainData;
        }
    }
}