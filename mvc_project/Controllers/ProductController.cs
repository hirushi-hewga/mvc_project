using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Products;
using mvc_project.Services.Image;
using mvc_project.Validators;

namespace mvc_project.Controllers
{
    public class ProductController(IProductRepository productRepository, IImageService imageService) : Controller
    {
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> IndexAsync()
        {
            var products = await productRepository.GetAllAsync();

            return View(products);
        }
        public async Task<IActionResult> EditAsync(string id)
        {
            var product = await productRepository.GetByIdAsync(id);
            
            if (product == null)
                return NotFound();

            var viewModel = new ProductCreateViewModel
            {
                Product = product,
                Categories = await productRepository.GetCategoriesSelectListAsync(),
                IsEdit = true
            };

            return View("Create", viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductCreateViewModel
            {
                Categories = await productRepository.GetCategoriesSelectListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([FromForm] ProductCreateViewModel viewModel)
        {
            var validator = new ProductValidator();
            var result = await validator.ValidateAsync(viewModel.Product);
            
            if (!result.IsValid)
            {
                viewModel.Categories = await productRepository.GetCategoriesSelectListAsync();
                viewModel.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View(viewModel);
            }

            string? fileName = null;
            if (viewModel.File != null)
            {
                fileName = await imageService.SaveImageAsync(viewModel.File, Settings.PRODUCTS_PATH);
            }
            viewModel.Product.Image = fileName;
            viewModel.Product.Id = Guid.NewGuid().ToString();

            await productRepository.CreateAsync(viewModel.Product);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(ProductCreateViewModel viewModel)
        {
            var validator = new ProductValidator();
            var result = await validator.ValidateAsync(viewModel.Product);

            if (!result.IsValid)
            {
                viewModel.Categories = await productRepository.GetCategoriesSelectListAsync();
                viewModel.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                viewModel.IsEdit = true;
                return View("Create", viewModel);
            }

            await productRepository.UpdateAsync(viewModel.Product);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Product model)
        {
            if (model.Image != null)
            {
                imageService.DeleteImage(Path.Combine(Settings.PRODUCTS_PATH, model.Image));
            }
            
            if (model.Id == null)
                return NotFound();

            await productRepository.DeleteAsync(model.Id);

            return RedirectToAction("Index");
        }
    }
}
