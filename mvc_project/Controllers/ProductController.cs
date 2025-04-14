using Microsoft.AspNetCore.Mvc;

namespace mvc_project.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            //Console.WriteLine("Products");

            return View();
        }
    }
}
