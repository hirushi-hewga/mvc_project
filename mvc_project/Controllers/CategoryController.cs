using Microsoft.AspNetCore.Mvc;
using mvc_project.Data;
using mvc_project.Models;

namespace mvc_project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.AsEnumerable();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            model.Id = Guid.NewGuid().ToString();
            _context.Categories.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
