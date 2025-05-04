using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class HomeIndexViewModel
    {
        public List<Product> Products { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public string? CategoryId { get; set; } = null;
    }
}