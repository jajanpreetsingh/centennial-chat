using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.Models.QuestionModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System;
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

            Question ques = questionRepository.GetByChatCodeNContent(questionDTO.chatCode, questionDTO.content);

            if (ques != null)
            {
                return new ResponseDTO(ResponseCode.ERROR, "Question with same content already exists in chat");
            }

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

        public ResponseDTO PublishQuestion(QuestionDTO question)
        {
            PollingQuestion ques = questionRepository.GetPollById(Guid.Parse(question.id));

            if (ques == null)
            {
                SubjectiveQuestion sub = questionRepository.GetOpenQuesById(Guid.Parse(question.id));

                if (sub == null)
                {
                    return new ResponseDTO(ResponseCode.ERROR, "Question doesnot exist");
                }
                else
                {
                    sub.IsPublished = true;
                    sub.PublishDate = DateTime.Now;

                    return new ResponseDTO(ResponseCode.OK, sub.GetDTO());
                }
            }

            ques.IsPublished = true;
            ques.PublishDate = DateTime.Now;

            return new ResponseDTO(ResponseCode.OK, ques.GetDTO());
        }

        public ResponseDTO ArchiveQuestion(QuestionDTO question)
        {
            PollingQuestion ques = questionRepository.GetPollById(Guid.Parse(question.id));

            if (ques == null)
            {
                SubjectiveQuestion sub = questionRepository.GetOpenQuesById(Guid.Parse(question.id));

                if (sub == null)
                {
                    return new ResponseDTO(ResponseCode.ERROR, "Question doesnot exist");
                }
                else
                {
                    sub.IsArchived = true;
                    sub.ArchiveDate = DateTime.Now;

                    return new ResponseDTO(ResponseCode.OK, sub.GetDTO());
                }
            }

            ques.IsArchived = true;
            ques.ArchiveDate = DateTime.Now;

            return new ResponseDTO(ResponseCode.OK, ques.GetDTO());
        }

        public ResponseDTO SaveAnswer(UserAnswerDTO answer)
        {
            List<UserAnswer> prevans = questionRepository.GetPreviousAnswers(answer);

            if (prevans != null && prevans.Count > 0)
                return new ResponseDTO(ResponseCode.ERROR, "Your answers to this question are already recorded");

            List<UserAnswer> answers = new List<UserAnswer>();

            if (!answer.isPollingQuestion)
            {
                answers.Add(new UserAnswer()
                {
                    MemberId = Guid.Parse(answer.memberId),
                    QuestionId = Guid.Parse(answer.questionId),
                    Content = answer.content,
                    ChatCode = answer.chatCode
                });

                questionRepository.SaveAnswers(answers);

                return new ResponseDTO(ResponseCode.OK, "Answers saved succesfully");
            }

            if (answer.options == null || answer.options.Length <= 0)
                return new ResponseDTO(ResponseCode.OK, "No options were selected");

            Guid qid = Guid.Parse(answer.questionId);
            Guid mid = Guid.Parse(answer.memberId);

            PollingQuestion ques = questionRepository.GetById(qid) as PollingQuestion;

            if (ques == null)
                return new ResponseDTO(ResponseCode.ERROR, "Corresponding Question not found");

            foreach (string option in answer.options)
            {
                QuestionOption opt = ques.Options.FirstOrDefault(x => x.Text == option);

                if (opt == null)
                    continue;

                answers.Add(new UserAnswer()
                {
                    MemberId = mid,
                    QuestionId = qid,
                    Content = opt.Text,
                    ChatCode = answer.chatCode,
                    OptionId = opt.OptionId
                });
            }

            questionRepository.SaveAnswers(answers);

            return new ResponseDTO(ResponseCode.OK, "Answers saved succesfully");
        }
    }
}