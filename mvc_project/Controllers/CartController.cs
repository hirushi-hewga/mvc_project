using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Services.Cart;
using mvc_project.Services.PromoCode;

namespace mvc_project.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IPromoCodeService _promoCodeService;

        public CartController(ICartService cartService, IPromoCodeService promoCodeService)
        {
            _cartService = cartService;
            _promoCodeService = promoCodeService;
        }

        public IActionResult Index()
        {
            var cartItems = _cartService.GetItems().ToList();
            var viewModel = new CartVM
            {
                CartItems = cartItems,
                Promocodes = _promoCodeService.GetAll().ToList(),
                Promocode = _promoCodeService.GetPromoCode(),
                Sum = cartItems.Sum(x => x.Price)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPromoCode(Promocode promocode)
        {
            _promoCodeService.SetPromoCode(promocode.Id);
            var cartItems = _cartService.GetItems().ToList();
            var viewModel = new CartVM
            {
                CartItems = cartItems,
                Promocodes = _promoCodeService.GetAll().ToList(),
                Promocode = _promoCodeService.GetPromoCode(),
                Sum = cartItems.Sum(x => x.Price)
            };
            return View("Index", viewModel);
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
            viewModel.CartItems = _cartService.GetItems().ToList();
            return View("Index", viewModel);
        }

        public async Task<IActionResult> PlaceOrderAsync(CartVM viewModel)
        {
            await _cartService.PlaceOrderAsync();
            viewModel.CartItems = _cartService.GetItems().ToList();
            return View("Index", viewModel);
        }
    }
}