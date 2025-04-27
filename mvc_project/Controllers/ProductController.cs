using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;

namespace mvc_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Index()
        {
            var products = _context.Products.
                Include(p => p.Category).
                AsEnumerable();

            return View(products);
        }
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new ProductCreateViewModel
            {
                Product = _context.Products.FirstOrDefault(p => p.Id == id),
                Categories = _context.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList(),
                IsEdit = true
            };

            if (viewModel.Product == null)
            {
                return NotFound();
            }

            return View("Create", viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new ProductCreateViewModel
            {
                Categories = _context.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ProductCreateViewModel viewModel)
        {
            var errors = ProductValidate(viewModel.Product);
            if (errors.Any())
            {
                viewModel.Categories = _context.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList();
                viewModel.Errors = errors.AsEnumerable();
                return View(viewModel);
            }

            string? fileName = null;
            if (viewModel.File != null)
            {
                fileName = SaveImage(viewModel.File);
            }
            viewModel.Product.Image = fileName;
            viewModel.Product.Id = Guid.NewGuid().ToString();

            _context.Products.Add(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductCreateViewModel viewModel)
        {
            var errors = ProductValidate(viewModel.Product);
            if (errors.Any())
            {
                viewModel.Categories = _context.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList();
                viewModel.Errors = errors.AsEnumerable();
                viewModel.IsEdit = true;
                return View("Create", viewModel);
            }

            _context.Products.Update(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product model)
        {
            if (model.Image != null)
            {
                var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, Settings.PRODUCTS_PATH);
                var imagePath = Path.Combine(imagesPath, model.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string? SaveImage(IFormFile file)
        {
            try
            {
                var types = file.ContentType.Split('/');

                if (types[0] != "image")
                {
                    return null;
                }

                string imageName = $"{Guid.NewGuid()}.{types[1]}";
                string imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, Settings.PRODUCTS_PATH);
                string imagePath = Path.Combine(imagesPath, imageName);

                using (var fileStream = System.IO.File.Create(imagePath))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
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
