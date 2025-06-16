using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_project.Models;
using mvc_project.Models.Identity;
using mvc_project.Repositories.Products;
using mvc_project.Services.Cart;
using NuGet.Protocol;

namespace mvc_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICartService _cartService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICartService cartService, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _productRepository = productRepository;
            _cartService = cartService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> IndexAsync(string? categoryId = "", int page = 1)
        {
            var products = (string.IsNullOrEmpty(categoryId)
                ? _productRepository.Products
                : _productRepository.GetByCategoryId(categoryId).Include(p => p.Category)
                ).OrderByDescending(p => p.Amount).AsQueryable();
            
            int pageSize = 12;
            int pagesCount = (int)Math.Ceiling(products.Count() / (double)pageSize);
            page = page <= 0 || page > pagesCount ? 1 : page;
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            var cartItems = _cartService.GetItems().Select(i => i.ProductId);
            
            var viewModel = new HomeIndexViewModel
            {
                Products = await products.Select(p => new HomeProductItemVM{ Product = p, InCart = cartItems.Contains(p.Id)}).ToListAsync(),
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

        public IActionResult AddToAdmin()
        {
            var user = User;
            if (user != null)
            {
                if(user. Identity != null && user. Identity. IsAuthenticated)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    
                    var appUser = _userManager.FindByIdAsync(userId ?? "").Result;
                    
                    if (appUser == null)
                        return RedirectToAction("Index");

                    if (!_roleManager.RoleExistsAsync("admin").Result)
                    {
                        _roleManager.CreateAsync(new IdentityRole
                        {
                            Name = "admin"
                        }).Wait();
                    }
                        
                    _userManager.AddToRoleAsync(appUser, "admin").Wait();
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
