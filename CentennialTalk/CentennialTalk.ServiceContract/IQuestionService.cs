using CentennialTalk.Models.DTOModels;
using System.Collections.Generic;

namespace CentennialTalk.ServiceContract
{
    public interface IQuestionService
    {
        ResponseDTO AddQuestionToChat(QuestionDTO questionDTO);

        ResponseDTO GetChatQuestions(string chatCode);
    }
}