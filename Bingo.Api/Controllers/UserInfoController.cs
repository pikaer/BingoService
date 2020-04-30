using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Controllers
{
    public class UserInfoController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}