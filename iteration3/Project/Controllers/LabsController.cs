using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class LabsController : Controller
    {
        public IActionResult Labs()
        {
            return View();
        }
    }
}
