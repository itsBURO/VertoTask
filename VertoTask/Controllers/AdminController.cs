using Microsoft.AspNetCore.Mvc;

namespace VertoTask.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
