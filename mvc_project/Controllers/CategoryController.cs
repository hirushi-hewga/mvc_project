using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Categories;

namespace mvc_project.Controllers
{
    public class CategoryController(ICategoryRepository categoryRepository) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        public async Task<IActionResult> EditAsync(string id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            
            if (category == null)
                return NotFound();
            
            var viewModel = new CategoryCreateViewModel
            {
                Category = category,
                IsEdit = true
            };

            return View("Create", viewModel);
        }

        public async Task<IActionResult> DeleteAsync(string id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CategoryCreateViewModel viewModel)
        {
            var errors = CategoryValidate(viewModel.Category);
            if (errors.Any())
            {
                viewModel.Errors = errors.AsEnumerable();
                return View(viewModel);
            }

            var category = viewModel.Category;
            category.Id = Guid.NewGuid().ToString();
            
            await categoryRepository.CreateAsync(category);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(CategoryCreateViewModel viewModel)
        {
            var errors = CategoryValidate(viewModel.Category);
            if (errors.Any())
            {
                viewModel.Errors = errors.AsEnumerable();
                viewModel.IsEdit = true;
                return View("Create", viewModel);
            }

            await categoryRepository.UpdateAsync(viewModel.Category);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Category model)
        {
            if (model.Id == null)
                return NotFound();
            
            await categoryRepository.DeleteAsync(model.Id);

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
