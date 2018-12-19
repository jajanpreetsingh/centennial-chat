using SautinSoft.Document;

namespace CentennialTalk.ServiceContract
{
    public interface IFileService
    {
        string CreateWordDocument(string chatCode);
    }
}