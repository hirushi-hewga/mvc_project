using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Products;
using mvc_project.Repositories.Promocodes;
using mvc_project.Services.Cart;
using mvc_project.Services.PromoCode;

namespace mvc_project.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IProductRepository _productRepository;
        private readonly IPromocodeRepository _promocodeRepository;

        public CartController(ICartService cartService, IPromoCodeService promoCodeService, IProductRepository productRepository, IPromocodeRepository promocodeRepository)
        {
            _cartService = cartService;
            _promoCodeService = promoCodeService;
            _productRepository = productRepository;
            _promocodeRepository = promocodeRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var cartItems = _cartService.GetItems().ToList();
            var items = cartItems.Select(i => _productRepository.FindById(i.ProductId)).ToList();
            var viewModel = new CartVM
            {
                Items = items.Select(x => new ProductCartVM{ Product = x, Quantity = cartItems.FirstOrDefault(i => i.ProductId == x.Id).Quantity }).ToList(),
                Promocodes = _promocodeRepository.GetAll().ToList(),
                Promocode = await _promoCodeService.GetPromoCodeAsync(),
                Sum = cartItems.Sum(x => x.Quantity * items.First(i => i.Id == x.ProductId).Price)
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddPromoCode([FromBody] Promocode promocode)
        {
            if (string.IsNullOrEmpty(promocode.Id))
                return BadRequest();
            
            _promoCodeService.SetPromoCode(promocode.Id);
            return Ok();
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
        
        public IActionResult ClearCart(CartVM viewModel)
        {
            _cartService.ClearCart();
            var cartItems = _cartService.GetItems().ToList();
            var items = cartItems.Select(i => _productRepository.FindById(i.ProductId)).ToList();
            viewModel.Items = items.Select(x => new ProductCartVM{ Product = x, Quantity = cartItems.FirstOrDefault(i => i.ProductId == x.Id).Quantity }).ToList();
            return View("Index", viewModel);
        }

        public async Task<IActionResult> PlaceOrderAsync(CartVM viewModel)
        {
            await _cartService.PlaceOrderAsync();
            var cartItems = _cartService.GetItems().ToList();
            var items = cartItems.Select(i => _productRepository.FindById(i.ProductId)).ToList();
            viewModel.Items = items.Select(x => new ProductCartVM{ Product = x, Quantity = cartItems.FirstOrDefault(i => i.ProductId == x.Id).Quantity }).ToList();
            return RedirectToAction("Index", "Home");
        }
    }
}