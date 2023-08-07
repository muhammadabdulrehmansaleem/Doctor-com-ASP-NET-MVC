using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Project.Models;
namespace Project.Views.Shared.component
{
    public class Appointment:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            DoctorComContext cx=new DoctorComContext();
            var Medicine=cx.Medicines.ToList();
            return View(Medicine);
        }
    }
}
