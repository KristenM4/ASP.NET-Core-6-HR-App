using Microsoft.AspNetCore.Mvc;

namespace SeaWolf.HR.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
