using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class CategoryCreateViewModel
    {
        public Category Category { get; set; } = new();
        public bool IsEdit { get; set; } = false;
        public IEnumerable<string>? Errors { get; set; } = null;
    }
}
