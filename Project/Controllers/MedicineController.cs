using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Project.Controllers
{
    public class MedicineController : Controller
    {
        public IActionResult MedicineDashBoard()
        {
            DoctorComContext cx = new DoctorComContext();
            List<Medicine> data = cx.Medicines.ToList();

            return View(data);
        }
        [HttpPost]
        public IActionResult SetMedicineIds(List<int> medicineIds)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = System.DateTime.Now.AddDays(1);

            if (HttpContext.Request.Cookies.TryGetValue("selecteditems", out string cookiesContent))
            {
                var existingIds = new List<int>();
                foreach (string idString in cookiesContent.Split(','))
                {
                    if (int.TryParse(idString, out int id))
                    {
                        existingIds.Add(id);
                    }
                }

                existingIds.AddRange(medicineIds);
                HttpContext.Response.Cookies.Append("selecteditems", string.Join(",", existingIds), option);
                
            }
            else
            {
                HttpContext.Response.Cookies.Append("selecteditems", string.Join(",", medicineIds), option);
            }

            return Json(new { success = true });

        }
        public IActionResult MedicineCart()
        {
            if (HttpContext.Request.Cookies.TryGetValue("selecteditems", out string cookiesContent))
            {
                string[] arr = cookiesContent.Split(',');

                List<int> medicineIds = new List<int>();
                foreach (string item in arr)
                {
                    if (int.TryParse(item, out int id))
                    {
                        medicineIds.Add(id);
                    }
                }
                List<Medicine> medicines = new List<Medicine>();
                foreach(int id in medicineIds)
                {
                    medicines.Add(Models.Repositories.RepositoryMedicine.getDataByID(id));
                    
                }

                // Retrieve the "idQuantity" cookie value
                string idQuantityCookie = Request.Cookies["idQuantity"];


                List<int> ids = new List<int>();
                List<int> quantities = new List<int>();

                if (idQuantityCookie != null)
                {
                    // Decode and deserialize the cookie value
                    string decodedCookie = Uri.UnescapeDataString(idQuantityCookie);
                    List<dynamic> idQuantityList = JsonConvert.DeserializeObject<List<dynamic>>(decodedCookie);

                    // Extract IDs and quantities from the list
                    foreach (var item in idQuantityList)
                    {
                        int id = item.id;
                        int quantity = item.quantity;

                        ids.Add(id);
                        quantities.Add(quantity);
                    }
                    foreach (var medicine in medicines)
                    {
                        for (int i = 0; i < ids.Count(); i++)
                        {
                            if (medicine.Id == ids[i])
                            {
                                medicine.Quantity = quantities[i];
                                Console.WriteLine($"id in ids is {ids[i]} and id of medicine is {medicine.Id} quantity in list {quantities[i]} and quantity is {medicine.Quantity}");
                            }
                        }
                    }
                    return View("MedicineCart", medicines);
                }
                else
                {
                    return View("MedicineCart", medicines);
                }
            }
            
            return RedirectToAction("Error");
        }
        [HttpPost]
        public IActionResult RemoveMedicine(int medicineIds)
        {
            Console.WriteLine($"The id is{medicineIds}");
            if (HttpContext.Request.Cookies.TryGetValue("selecteditems", out string cookiesContent))
            {
                // Split the cookie value and convert it to a list of integers
                var existingIds = new List<int>();
                foreach (string idString in cookiesContent.Split(','))
                {
                    if (int.TryParse(idString, out int id))
                    {
                        existingIds.Add(id);
                    }
                }
               
                // Remove the specified medicine ID from the list
                existingIds.Remove(medicineIds);
                // Update the cookie with the modified list of IDs
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(1);
                HttpContext.Response.Cookies.Append("selecteditems", string.Join(",", existingIds), options);

                return Ok(); // Return a success response
            }

            return NotFound(); // Return a not found response if the cookie does not exist
        }
        [HttpPost]
        public ActionResult Checkout(int medicineIds)
        {
            HttpContext.Response.Cookies.Delete("selecteditems");
            HttpContext.Response.Cookies.Delete("idQuantity");
            return View("Home", "Index");
        }
    }
    

}
