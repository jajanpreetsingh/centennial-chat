using CentennialTalk.Models.QuestionModels;
using System;
using System.Collections.Generic;

namespace CentennialTalk.PersistenceContract
{
    public interface IQuestionRepository
    {
        List<PollingQuestion> GetChatPollingQuestions(string chatCode);

        List<SubjectiveQuestion> GetChatSubjectiveQuestions(string chatCode);

        Question GetByChatCodeNContent(string chatCode, string content);

        Question GetById(Guid id);
    }
}