using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.Models.QuestionModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IChatRepository chatRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly ILogger<ChatService> logger;

        public QuestionService(IChatRepository chatRepository, ILogger<ChatService> logger,
                               IQuestionRepository questionRepository)
        {
            this.chatRepository = chatRepository;
            this.questionRepository = questionRepository;
            this.logger = logger;
        }

        public ResponseDTO AddQuestionToChat(QuestionDTO questionDTO)
        {
            Discussion chat = chatRepository.GetChatByCode(questionDTO.chatCode);

            if (chat == null)
                return new ResponseDTO(ResponseCode.ERROR, "chat does not exist");

            if (questionDTO.isPollingQuestion)
            {
                PollingQuestion pollingQuestion = new PollingQuestion(questionDTO);

                pollingQuestion.ChatCode = questionDTO.chatCode;

                chat.Polls.Add(pollingQuestion);
            }
            else
            {
                SubjectiveQuestion subjectiveQuestion = new SubjectiveQuestion(questionDTO);

                subjectiveQuestion.ChatCode = questionDTO.chatCode;

                chat.Questions.Add(subjectiveQuestion);
            }

            return new ResponseDTO(ResponseCode.ERROR, "Question added successfully");
        }

        public ResponseDTO GetChatQuestions(string chatCode)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();

            List<PollingQuestion> polls = questionRepository.GetChatPollingQuestions(chatCode);

            List<SubjectiveQuestion> subjectives = questionRepository.GetChatSubjectiveQuestions(chatCode);

            if (polls != null)
                polls.ToList().ForEach(x => questions.Add(x.GetDTO()));

            if (subjectives != null)
                subjectives.ToList().ForEach(x => questions.Add(x.GetDTO()));

            if (questions.Count <= 0)
                return new ResponseDTO(ResponseCode.OK, "No questions exist for this chat");

            return new ResponseDTO(ResponseCode.OK, questions);
        }
    }
}