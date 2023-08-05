using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Services()
        {
            return View();
        }
    }
}
