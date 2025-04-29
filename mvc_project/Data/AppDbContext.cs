using Microsoft.EntityFrameworkCore;
using mvc_project.Models;

namespace mvc_project.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
