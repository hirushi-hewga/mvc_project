using mvc_project.Data;
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
            var product = _productRepository.FindById(viewModel.ProductId);
            if (product == null)
                return;
            
            var list = GetItems().ToList();

            if (list.Exists(i => i.ProductId == viewModel.ProductId))
            {
                var index = list.FindIndex(i => i.ProductId == viewModel.ProductId);
                if (list[index].Quantity == product.Amount)
                    return;
                list[index].Quantity++;
            }
            else
            {
                if (product.Amount <= 0)
                    return;
                list.Add(viewModel);
            }
            
            SetItems(list);
        }

        public void RemoveFromCart(CartItemVM viewModel)
        {
            var list = GetItems().ToList();

            if (list.FirstOrDefault(i => i.ProductId == viewModel.ProductId)?.Quantity > 1 && viewModel.Quantity != 1)
            {
                var index = list.FindIndex(i => i.ProductId == viewModel.ProductId);
                list[index].Quantity--;
            }
            else
            {
                list = list.Where(i => i.ProductId != viewModel.ProductId).ToList();
            }
            
            SetItems(list);
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

        public void SetItems(IEnumerable<CartItemVM> items)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            var session = context.Session;
            
            session.Set(Settings.SessionCartKey, items);
        }

        public CartItemVM? FindById(string id)
        {
            var items = GetItems();
            var item = items.FirstOrDefault(i => i.ProductId == id);
            return item;
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
        
        public async Task PlaceOrderAsync()
        {
            var items = GetItems().Select(i => _productRepository.FindById(i.ProductId)).ToArray();
            foreach (var item in items)
            {
                var cartItem = FindById(item.Id);
                if (cartItem == null)
                    return;
                item.Amount -= cartItem.Quantity;
            }
            await _productRepository.UpdateAsync(items);
            ClearCart();
        }
    }
}