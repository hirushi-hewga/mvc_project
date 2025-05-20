using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_project.Models
{
    public class HomeIndexViewModel
    {
        public List<HomeProductItemVM> Products { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public string? CategoryId { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PagesCount { get; set; }
    }
}