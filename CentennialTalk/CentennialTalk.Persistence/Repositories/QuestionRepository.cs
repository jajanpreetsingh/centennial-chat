using CentennialTalk.Models.QuestionModels;
using CentennialTalk.PersistenceContract;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Persistence.Repositories
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ChatDBContext context) : base(context)
        {
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