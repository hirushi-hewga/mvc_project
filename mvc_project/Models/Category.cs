using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mvc_project.Models
{
    public class Category : BaseModel<string>
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}