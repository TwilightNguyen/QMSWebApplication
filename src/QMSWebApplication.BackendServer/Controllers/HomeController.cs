using Microsoft.AspNetCore.Mvc;

namespace QMSWebApplication.BackendServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
