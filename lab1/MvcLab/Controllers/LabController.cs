using Microsoft.AspNetCore.Mvc;

namespace MvcLab.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Info()
        {
            
            ViewData["LabNumber"] = "Лабораторна робота №1";
            ViewData["Topic"] = "Створення проектів ASP.NET з використанням dotnet CLI";
            ViewData["Goal"] = "Навчитись створювати WebAPI та MVC-додатки через CLI";
            ViewData["Author"] = "Олександр Ганчевський";

            return View();
        }
    }
}
