using mvc_project.Models;
using mvc_project.Repositories.Products;
using mvc_project.Services.Session;

namespace mvc_project.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;

        public CartService(IHttpContextAccessor httpContextAccessor, IProductRepository productRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
        }
        
        public void AddToCart(CartItemVM viewModel)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            var session = context.Session;

            var items = session.Get<IEnumerable<CartItemVM>>(Settings.SessionCartKey);
            var list = items == null ? new List<CartItemVM>() : items.ToList();

            if (list.Exists(i => i.ProductId == viewModel.ProductId))
            {
                var index = list.FindIndex(i => i.ProductId == viewModel.ProductId);
                if (list[index].Quantity == list[index].Amount)
                    return;
                list[index].Price += list[index].Price / list[index].Quantity;
                list[index].Quantity++;
            }
            else
            {
                viewModel.Amount = _productRepository.Products.FirstOrDefault(p => p.Id == viewModel.ProductId).Amount;
                list.Add(viewModel);
            }
            
            session.Set<IEnumerable<CartItemVM>>(Settings.SessionCartKey, list);
        }

        public void RemoveFromCart(CartItemVM viewModel)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            var session = context.Session;

            var items = session.Get<IEnumerable<CartItemVM>>(Settings.SessionCartKey);
            items ??= new List<CartItemVM>();
            var list = items.ToList();

            if (list.FirstOrDefault(i => i.ProductId == viewModel.ProductId)?.Quantity > 1 && viewModel.Quantity != 1)
            {
                var index = list.FindIndex(i => i.ProductId == viewModel.ProductId);
                list[index].Price -= list[index].Price / list[index].Quantity;
                list[index].Quantity--;
            }
            else
            {
                list = items.Where(i => i.ProductId != viewModel.ProductId).ToList();
            }
            
            session.Set<IEnumerable<CartItemVM>>(Settings.SessionCartKey, list);
        }

        public IEnumerable<CartItemVM> GetItems()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return new List<CartItemVM>();
            var session = context.Session;
            
            var items = session.Get<IEnumerable<CartItemVM>>(Settings.SessionCartKey);
            return items ?? new List<CartItemVM>();
        }

        public static int GetCount(HttpContext? context)
        {
            if (context == null)
                return 0;
            var session = context.Session;
            var items = session.Get<IEnumerable<CartItemVM>>(Settings.SessionCartKey);
            return items == null ? 0 : items.Count();
        }

        public void ClearCart()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            context.Session.Clear();
        }
    }
}