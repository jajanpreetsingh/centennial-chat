using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentennialTalk.Main.Controllers
{
    public class BaseController : Controller
    {
        public JsonResult GetJson(object data)
        {
            return new JsonResult(data);
        }
    }
}
