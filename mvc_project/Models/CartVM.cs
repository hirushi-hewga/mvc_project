namespace mvc_project.Models
{
    public class CartVM
    {
        public List<Product> Items { get; set; } = new();
        public List <CartItemVM> CartItems { get; set; } = new();
        public List<Promocode> Promocodes { get; set; }
        public Promocode Promocode { get; set; } = new();
        public decimal Sum { get; set; } = 0;
    }
}