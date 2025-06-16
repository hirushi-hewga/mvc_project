namespace mvc_project.Models
{
    public class PromocodeCreateVM
    {
        public Promocode Promocode { get; set; } = new();
        public bool IsEdit { get; set; } = false;
        public IEnumerable<string>? Errors { get; set; } = null;
    }
}