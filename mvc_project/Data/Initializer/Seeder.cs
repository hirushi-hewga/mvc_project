using Microsoft.EntityFrameworkCore;
using mvc_project.Models;

namespace mvc_project.Data.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                context.Database.Migrate();

                if (!context.Categories.Any())
                {
                    var categories = new List<Category>
                    {
                        new Category{ Id = Guid.NewGuid().ToString(), Name = "Процесори" },
                        new Category{ Id = Guid.NewGuid().ToString(), Name = "Материнські плати" },
                        new Category{ Id = Guid.NewGuid().ToString(), Name = "Блоки живлення" },
                        new Category{ Id = Guid.NewGuid().ToString(), Name = "Оперативна пам'ять" },
                        new Category{ Id = Guid.NewGuid().ToString(), Name = "Відеокарти" },
                        new Category{ Id = Guid.NewGuid().ToString(), Name = "Накопичувачі" }
                    };
                    
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                    
                    var products = new List<Product>
                    {
                        new Product
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "AMD Ryzen 5 5600X",
                            Price = 4500,
                            Amount = 17,
                            CategoryId = categories[0].Id
                        },
                        new Product
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "NVIDIA GeForce RTX 4060",
                            Price = 13500,
                            Amount = 13,
                            CategoryId = categories[4].Id
                        }
                    };
                }
            }
        }
    }
}