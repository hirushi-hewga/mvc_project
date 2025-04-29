using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Products;
using mvc_project.Services.Image;

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
            var errors = ProductValidate(viewModel.Product);
            if (errors.Any())
            {
                viewModel.Categories = await productRepository.GetCategoriesSelectListAsync();
                viewModel.Errors = errors.AsEnumerable();
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
            var errors = ProductValidate(viewModel.Product);
            if (errors.Any())
            {
                viewModel.Categories = await productRepository.GetCategoriesSelectListAsync();
                viewModel.Errors = errors.AsEnumerable();
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

        private List<string> ProductValidate(Product product)
        {
            var errors = new List<string>();

            // Name
            if (string.IsNullOrWhiteSpace(product.Name)) { errors.Add("Назва є обов'язковою"); }
            else if (product.Name?.Length > 100) { errors.Add("Назва має бути менше ніж 100 символів"); }

            // Description
            if (!string.IsNullOrWhiteSpace(product.Description) && product.Description.Length > 255)
            { errors.Add("Опис має бути менше ніж 255 символів"); }

            // Price
            if (product.Price < 0) { errors.Add("Ціна має бути не меншою ніж 0"); }

            // Amount
            if (product.Amount < 0) { errors.Add("Кількість продуктів має бути не менше ніж 0"); }

            return errors;
        }
    }
}
