using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class CategoryCreateViewModel
    {
        public Category Category { get; set; }
        public bool IsEdit { get; set; } = false;
    }
}
