using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Blogs()
        {
            return View();
        }
    }
}
