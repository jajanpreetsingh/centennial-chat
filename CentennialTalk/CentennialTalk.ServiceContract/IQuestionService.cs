﻿using CentennialTalk.Models.DTOModels;

namespace CentennialTalk.ServiceContract
{
    public interface IQuestionService
    {
        ResponseDTO AddQuestionToChat(QuestionDTO questionDTO);

        ResponseDTO GetChatQuestions(string chatCode);

        ResponseDTO PublishQuestion(QuestionDTO question);

        ResponseDTO ArchiveQuestion(QuestionDTO question);

        ResponseDTO SaveAnswer(UserAnswerDTO answer);
    }
}