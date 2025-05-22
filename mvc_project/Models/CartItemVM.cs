namespace mvc_project.Models
{
    public class CartItemVM
    {
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public int Amount { get; set; } = 0;
    }
}