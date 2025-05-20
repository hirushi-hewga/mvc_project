using mvc_project.Models;
using mvc_project.Services.Session;

namespace mvc_project.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public void AddToCart(CartItemVM viewModel)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            var session = context.Session;

            var items = session.Get<IEnumerable<CartItemVM>>(Settings.SessionCartKey);
            var list = items == null ? new List<CartItemVM>() : items.ToList();
                
            list.Add(viewModel);
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
                
            items = items.Where(i => i.ProductId != viewModel.ProductId);
            
            session.Set<IEnumerable<CartItemVM>>(Settings.SessionCartKey, items);
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
    }
}