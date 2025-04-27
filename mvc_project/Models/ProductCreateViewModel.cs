using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public IFormFile? File { get; set; }
        public bool IsEdit { get; set; } = false;
        public IEnumerable<string>? Errors { get; set; } = null;
    }
}
