using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class ProductReadViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
