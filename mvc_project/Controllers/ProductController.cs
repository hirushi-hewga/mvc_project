using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_project.Data;
using mvc_project.Models;

namespace mvc_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Delete(string id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            return View(product);
        }

        public IActionResult Index()
        {
            var viewModel = new ProductReadViewModel
            {
                Products = _context.Products.AsEnumerable(),
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }
        public IActionResult Edit(string id)
        {
            var viewModel = new ProductCreateViewModel
            {
                Product = _context.Products.FirstOrDefault(p => p.Id == id),
                Categories = _context.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList(),
                IsEdit = true
            };
            return View("Create", viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new ProductCreateViewModel
            {
                Product = new Product(),
                Categories = _context.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateViewModel model)
        {
            var product = model.Product;
            product.Id = Guid.NewGuid().ToString();
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductCreateViewModel model)
        {
            _context.Products.Update(model.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product model)
        {
            _context.Products.Remove(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
