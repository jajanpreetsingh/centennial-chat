using Microsoft.AspNetCore.Mvc;

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