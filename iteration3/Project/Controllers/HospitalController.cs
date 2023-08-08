using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class HospitalController : Controller
    {
        public IActionResult Hospital()
        {
            return View();
        }
    }
}
