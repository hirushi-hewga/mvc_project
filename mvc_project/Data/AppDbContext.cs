using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc_project.Models;
using mvc_project.Models.Identity;

namespace mvc_project.Data
{
    public class AppDbContext(DbContextOptions options)
        : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
    }
}
