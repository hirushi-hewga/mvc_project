using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public bool IsEdit { get; set; } = false;
    }
}
