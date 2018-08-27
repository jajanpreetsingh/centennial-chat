using CentennialTalk.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/file")]
    public class FileController : BaseController
    {
        private const string fileName = "D:\\data\\soundMessage.raw";

        [HttpPost("save")]
        public IActionResult SaveToFile([FromBody]RequestDTO data)
        {
            if (!System.IO.File.Exists(fileName))
            {
                FileStream fs = System.IO.File.Create(fileName);
                fs.Close();
            }

            byte[] b = Convert.FromBase64String(data.value.ToString());
            System.IO.File.WriteAllBytes(fileName, b);

            return new JsonResult(true);
        }
    }
}