using mvc_project.Models;

namespace mvc_project.Services.Cart
{
    public interface ICartService
    {
        void AddToCart(CartItemVM viewModel);
        void RemoveFromCart(CartItemVM viewModel);
        IEnumerable<CartItemVM> GetItems();
        void ClearCart();
        Task PlaceOrderAsync();
    }
}