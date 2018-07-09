using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/file")]
    public class FileController : BaseController
    {
        private const string fileName = "D:\\data\\soundMessage.raw";

        [HttpPost("save")]
        public IActionResult SaveToFile(object data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, data);

            if (!System.IO.File.Exists(fileName))
            {
                FileStream fs = System.IO.File.Create(fileName);
                fs.Close();
            }

            System.IO.File.WriteAllBytes(fileName, ms.ToArray());

            return new JsonResult(true);
        }
    }
}