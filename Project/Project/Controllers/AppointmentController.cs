using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Appointment()
        {
            return View();
        }
    }
}
