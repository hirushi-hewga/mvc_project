using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(new CategoryCreateViewModel());
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryCreateViewModel
            {
                Category = _context.Categories.FirstOrDefault(c => c.Id == id),
                IsEdit = true
            };

            if (viewModel.Category == null)
            {
                return NotFound();
            }

            return View("Create", viewModel);
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryCreateViewModel viewModel)
        {
            var errors = CategoryValidate(viewModel.Category);
            if (errors.Any())
            {
                viewModel.Errors = errors.AsEnumerable();
                return View(viewModel);
            }

            var category = viewModel.Category;
            category.Id = Guid.NewGuid().ToString();
            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryCreateViewModel viewModel)
        {
            var errors = CategoryValidate(viewModel.Category);
            if (errors.Any())
            {
                viewModel.Errors = errors.AsEnumerable();
                viewModel.IsEdit = true;
                return View("Create", viewModel);
            }

            _context.Categories.Update(viewModel.Category);
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

        private List<string> CategoryValidate(Category category)
        {
            var errors = new List<string>();

            // Name
            if (string.IsNullOrWhiteSpace(category.Name)) { errors.Add("Назва є обов'язковою"); }
            else if (category.Name?.Length > 100) { errors.Add("Назва має бути менше ніж 100 символів"); }

            return errors;
        }
    }
}
