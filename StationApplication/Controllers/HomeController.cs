using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace StationApplication.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404 || statusCode.Value == 500)
                {
                    Logger.LogError(statusCode.ToString() + " Requested page not found!");
                    var viewName = statusCode.ToString();
                    return View(viewName);
                }
            }
            return View();
        }
    }
}