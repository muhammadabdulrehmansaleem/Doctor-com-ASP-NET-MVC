using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Newtonsoft.Json;

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
        public IActionResult Signup_Patient()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup_Patient_Successful(Patient s)
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
            return View("Login_Patient", "Login_Signup");
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
            return View("Login_Doctor","Login_Signup");

        }
        public IActionResult Login_Patient()
        {
            return View();
        }
        public IActionResult Login_Doctor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login_Patient_Successful(Patient s)
        {
            DoctorComContext cx = new DoctorComContext();
            List<Patient> data = cx.Patients.ToList();
            foreach (Patient pat in data)
            {
                if (pat.Email == s.Email && pat.Password == s.Password)
                {
                    string userJson = JsonConvert.SerializeObject(pat);
                    // Create a cookie to store the patient data
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1) // Set cookie expiration to 1 day
                    };
                    Response.Cookies.Append("UserInfo", userJson, cookieOptions);

                    return View("~/Views/Home/Index.cshtml");
                }
            }
            ViewBag.Message = "Password or email is incorrect";
            return View("~/Views/Login_Signup/Login_Patient.cshtml");
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
                    string userJson = JsonConvert.SerializeObject(pat);
                    // Create a cookie to store the patient data
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1) // Set cookie expiration to 1 day
                    };
                    Response.Cookies.Append("UserInfo", userJson, cookieOptions);

                    return View("~/Views/Home/Index.cshtml");
                }
            }
            ViewBag.Message = "Password or email is incorrect";
            return View("~/Views/Login_Signup/Login_Doctor.cshtml");
        }
        public IActionResult ClearPatientCookie()
        {
            // Delete the "PatientInfo" cookie
            Response.Cookies.Delete("UserInfo");

            return RedirectToAction("Index", "Home"); // Redirect to home page or any other page
        }
    }
}
