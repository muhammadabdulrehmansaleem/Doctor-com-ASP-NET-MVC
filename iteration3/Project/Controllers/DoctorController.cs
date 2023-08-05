using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Doctor()
        {
            return View();
        }
    }
}
