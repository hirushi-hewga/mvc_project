using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc_project.Models;

namespace mvc_project.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
    }
}
