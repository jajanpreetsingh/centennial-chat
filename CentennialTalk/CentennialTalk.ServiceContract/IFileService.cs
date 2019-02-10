using CentennialTalk.Models.DTOModels;

namespace CentennialTalk.ServiceContract
{
    public interface IFileService
    {
        string CreateWordDocument(TranscriptRequestDTO trm);
    }
}