using CentennialTalk.Models.DTOModels;
using Google.Cloud.Speech.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/file")]
    public class FileController : BaseController
    {
        private const string fileName = "D:\\data\\soundMessage.wav";

        [HttpPost("save")]
        public async Task<IActionResult> SaveToFile([FromForm] IFormFile speech)
        {
            long totalSize = speech.Length;
            byte[] fileBytes = new byte[speech.Length];

            using (Stream fileStream = speech.OpenReadStream())
            {
                int offset = 0;

                while (offset < speech.Length)
                {
                    int chunkSize = totalSize - offset < 8192 ? (int)totalSize - offset : 8192;

                    offset += await fileStream.ReadAsync(fileBytes, offset, chunkSize);
                }
            }

            System.IO.File.WriteAllBytes(fileName, fileBytes);

            string result = GetSpeechToText();

            if (string.IsNullOrWhiteSpace(result))
                return GetJson(new ResponseDTO(ResponseCode.ERROR, "Error in speech conversion"));
            else
                return GetJson(new ResponseDTO(ResponseCode.OK, result));
        }

        private string GetSpeechToText()
        {
            string result = string.Empty;

            try
            {
                SpeechClient speechClient = SpeechClient.Create();

                RecognitionConfig config = new RecognitionConfig()
                {
                    //Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    //SampleRateHertz = 16000,
                    LanguageCode = "en-US",
                };

                RecognizeResponse response = speechClient.Recognize(config,
                    RecognitionAudio.FromFile(fileName));

                foreach (var responseResult in response.Results)
                {
                    foreach (var alternative in responseResult.Alternatives)
                    {
                        result += alternative.Transcript;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }

            return result;
        }
    }
}