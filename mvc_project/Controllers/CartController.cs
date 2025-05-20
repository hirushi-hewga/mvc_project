using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Products;
using mvc_project.Services.Cart;

namespace mvc_project.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;

        public CartController(ICartService cartService, IProductRepository productRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToCartAsync([FromBody] CartItemVM viewModel)
        {
            var product = await _productRepository.FindByIdAsync(viewModel.ProductId);
            
            if (string.IsNullOrEmpty(viewModel.ProductId) || product?.Amount <= 0)
                return BadRequest();
            
            _cartService.AddToCart(viewModel);
            return Ok();
        }
        
        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
                return BadRequest();
            
            _cartService.RemoveFromCart(viewModel);
            return Ok();
        }
    }
}