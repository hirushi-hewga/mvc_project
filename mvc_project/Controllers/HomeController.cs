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

        public async Task<IActionResult> IndexAsync()
        {
            var viewModel = new HomeIndexViewModel
            {
                Products = await _productRepository.Products.ToListAsync(),
                Categories = await _productRepository.GetCategoriesSelectListAsync()
            };
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexAsync(HomeIndexViewModel viewModel)
        {
            viewModel.Categories = await _productRepository.GetCategoriesSelectListAsync();
            if (viewModel.CategoryId == null)
            {
                return View(viewModel);
            }
            
            viewModel.Products = await _productRepository.GetByCategoryIdAsync(viewModel.CategoryId);
            return View(viewModel);
        }
    }
}
