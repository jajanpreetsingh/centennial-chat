using CentennialTalk.Models.DTOModels;
using CentennialTalk.Models.QuestionModels;

namespace CentennialTalk.ServiceContract
{
    public interface IQuestionService
    {
        ResponseDTO AddQuestionToChat(QuestionDTO questionDTO);

        ResponseDTO GetChatQuestions(string chatCode);

        ResponseDTO PublishQuestion(QuestionDTO question);

        ResponseDTO ArchiveQuestion(QuestionDTO question);

        ResponseDTO SaveAnswer(UserAnswerDTO answer);

        ClusterPrediction TrainModel(ResponseTrainingModel predictArg);
    }
}