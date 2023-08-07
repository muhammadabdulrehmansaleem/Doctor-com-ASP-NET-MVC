using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class Login_SignupController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        public IActionResult Signup_Doctor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup_Successful(Patient s)
        {

            DoctorComContext cx = new DoctorComContext();
            List<Patient> data = cx.Patients.ToList();
            foreach (Patient pat in data)
            {
                if (pat.Email == s.Email)
                {
                    Console.WriteLine("aaaa");
                    ViewBag.Message = "This email already has account";
                    return View("~/Views/Login_Signup/Signup.cshtml");
                }
            }
            cx.Patients.Add(s);
            cx.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }
        [HttpPost]
        public IActionResult Signup_Doctor_Succsessful(Doctor s)
        {
            DoctorComContext cx = new DoctorComContext();
            List<Doctor> data = cx.Doctors.ToList();
            foreach (Doctor pat in data)
            {
                if (pat.Email == s.Email)
                {
                    ViewBag.Message = "This email already has account";
                    return View("~/Views/Login_Signup/Signup_doctor.cshtml");
                }
            }
            cx.Doctors.Add(s);
            cx.SaveChanges();
            return View("~/Views/Home/Index.cshtml");

        }
        public IActionResult Login_Patients()
        {
            return View();
        }
        public IActionResult Login_Doctor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login_Patients_Successful(Patient s)
        {
            DoctorComContext cx = new DoctorComContext();
            List<Patient> data = cx.Patients.ToList();
            foreach (Patient pat in data)
            {
                if (pat.Email == s.Email && pat.Password == s.Password)
                {
                    return View("~/Views/Home/Index.cshtml");
                }
            }
            ViewBag.Message = "Password or email is incorrect";
            return View("~/Views/Login_Signup/Login_Patients.cshtml");
        }
        [HttpPost]
        public IActionResult Login_Doctor_Successful(Doctor s)
        {
            DoctorComContext cx = new DoctorComContext();
            List<Doctor> data = cx.Doctors.ToList();
            foreach (Doctor pat in data)
            {
                if (pat.Email == s.Email && pat.Password == s.Password)
                {
                    return View("~/Views/Home/Index.cshtml");
                }
            }
            ViewBag.Message = "Password or email is incorrect";
            return View("~/Views/Login_Signup/Login_Doctor.cshtml");
        }
    }
}
