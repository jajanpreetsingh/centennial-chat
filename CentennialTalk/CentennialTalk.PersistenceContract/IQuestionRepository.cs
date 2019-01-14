using CentennialTalk.Models.DTOModels;
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

        int SaveAnswers(List<UserAnswer> answers);

        List<UserAnswer> GetAnswersByChat(string chatCode);

        PollingQuestion GetPollById(Guid id);

        SubjectiveQuestion GetOpenQuesById(Guid id);

        List<UserAnswer> GetPreviousAnswers(UserAnswerDTO answer);

        List<QuestionTrainingModel> GetAllSubjectiveAnswers();
    }
}