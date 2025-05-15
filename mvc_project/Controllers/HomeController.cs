using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_project.Models;
using mvc_project.Repositories.Products;

namespace mvc_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> IndexAsync(string? categoryId = "", int page = 1)
        {
            var products = string.IsNullOrEmpty(categoryId)
                ? _productRepository.Products
                : _productRepository.GetByCategoryId(categoryId).Include(p => p.Category);
            
            int pageSize = 12;
            int pagesCount = (int)Math.Ceiling(products.Count() / (double)pageSize);
            page = page <= 0 || page > pagesCount ? 1 : page;
            products = products.Skip((page - 1) * pageSize).Take(pageSize);
            
            var viewModel = new HomeIndexViewModel
            {
                Products = await products.ToListAsync(),
                Categories = await _productRepository.GetCategoriesSelectListAsync(),
                CategoryId = categoryId,
                Page = page,
                PagesCount = pagesCount
            };
            
            var pages = viewModel.Products.Count / pageSize;
            
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ActionName("Details")]
        public async Task<IActionResult> ProductDetailsAsync(string? id)
        {
            if (id == null)
                return NotFound();

            var model = await _productRepository.FindByIdAsync(id);
            
            return View("ProductDetails", model);
        }

        public IActionResult Info()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
