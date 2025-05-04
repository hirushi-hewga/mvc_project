using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Categories;
using mvc_project.Validators;

namespace mvc_project.Controllers
{
    public class CategoryController(ICategoryRepository categoryRepository) : Controller
    {
        public IActionResult Index()
        {
            var categories = categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        public async Task<IActionResult> EditAsync(string id)
        {
            var category = await categoryRepository.FindByIdAsync(id);
            
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
            var category = await categoryRepository.FindByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CategoryCreateViewModel viewModel)
        {
            var validator = new CategoryValidator();
            var result = await validator.ValidateAsync(viewModel.Category);
            
            if (!result.IsValid)
            {
                viewModel.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View(viewModel);
            }

            viewModel.Category.Id = Guid.NewGuid().ToString();
            
            await categoryRepository.CreateAsync(viewModel.Category);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(CategoryCreateViewModel viewModel)
        {
            var validator = new CategoryValidator();
            var result = await validator.ValidateAsync(viewModel.Category);
            
            if (!result.IsValid)
            {
                viewModel.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
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
    }
}
