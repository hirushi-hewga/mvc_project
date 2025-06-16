namespace mvc_project.Models
{
    public class ProductCartVM
    {
        public Product Product { get; set; } = new();
        public int Quantity { get; set; } = 1;
    }
}