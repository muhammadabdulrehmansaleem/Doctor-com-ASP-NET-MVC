using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Project.Models;
namespace Project.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Medicine()
        {
            return View();
        }
        public IActionResult Doctor()
        {
            DoctorComContext cx = new DoctorComContext();
            List<Doctor> data = cx.Doctors.ToList();
            return View(data);
        }
        public IActionResult Hospital()
        {
            return View();
        }
        public IActionResult Blogs()
        {
            return View();
        }
        public IActionResult Labs_home()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Labs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Labs(DoctorAppointment appointment)
        {
            appointment.AppointmentType = "Lab";
            DoctorComContext doctorComContext = new DoctorComContext();
            doctorComContext.Add(appointment);
            doctorComContext.SaveChanges();
            return View("Success");
        }
        [HttpGet]
        public IActionResult Appointment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Appointment(DoctorAppointment appointment)
        {
            appointment.AppointmentType = "Doctor";
            DoctorComContext doctorComContext = new DoctorComContext();
            doctorComContext.Add(appointment);
            doctorComContext.SaveChanges();
            return View("Success");
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult DoctorInbox(int userId)
        {
            
            try
            {
                using (var dbContext = new DoctorComContext())
                {
                    var distinctPatients = dbContext.Chats
                        .Where(c => c.DoctorId == userId && c.UserRole == "Patient")
                        .GroupBy(c => c.PatientId)
                        .Select(g => g.OrderByDescending(c => c.MessageTimestamp).FirstOrDefault())
                        .ToList();

                    return View(distinctPatients);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting patients: {ex.Message}");
                return View(new List<Chat>()); // Return an empty list if there's an error
            }
        }
    }   
}
