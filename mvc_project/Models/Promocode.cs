using System.ComponentModel.DataAnnotations;

namespace mvc_project.Models
{
    public class Promocode : BaseModel<string>
    {
        [Range(0, 100)]
        public int Discount { get; set; } = 0;
    }
}