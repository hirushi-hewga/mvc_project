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
            var items = _cartService.GetItems().ToList();
            
            return View(items);
        }
        
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
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
        
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return View("Index", _cartService.GetItems().ToList());
        }

        public async Task<IActionResult> PlaceOrderAsync()
        {
            await _cartService.PlaceOrderAsync();
            return View("Index", _cartService.GetItems().ToList());
        }
    }
}