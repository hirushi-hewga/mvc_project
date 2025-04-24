using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var viewModel = new CategoryCreateViewModel
            {
                Category = new Category(),
            };
            return View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            var viewModel = new CategoryCreateViewModel
            {
                Category = _context.Categories.FirstOrDefault(c => c.Id == id),
                IsEdit = true
            };
            return View("Create", viewModel);
        }

        public IActionResult Delete(string id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            var category = model.Category;
            category.Id = Guid.NewGuid().ToString();
            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryCreateViewModel model)
        {
            _context.Categories.Update(model.Category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category model)
        {
            _context.Categories.Remove(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
