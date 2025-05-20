namespace mvc_project.Models
{
    public class HomeProductItemVM
    {
        public Product Product { get; set; } = new();
        public bool InCart { get; set; } = false;
    }
}