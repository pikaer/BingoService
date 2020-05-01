using Microsoft.AspNetCore.Mvc;

namespace Bingo.Web.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}