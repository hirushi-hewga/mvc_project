using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc_project.Models;
using mvc_project.Models.Identity;

namespace mvc_project.Data.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                
                context.Database.Migrate();

                if (!context.Promocodes.Any())
                {
                    var promocodes = new List<Promocode>
                    {
                        new Promocode{ Id = "SAVE10NOW", Discount = 10 },
                        new Promocode{ Id = "FLASH15OFF", Discount = 15 },
                        new Promocode{ Id = "DISCOUNT20", Discount = 20 },
                        new Promocode{ Id = "BIGSALE30", Discount = 30 },
                        new Promocode{ Id = "HALFPRICE", Discount = 50 }
                    };
                    
                    context.Promocodes.AddRange(promocodes);
                    context.SaveChanges();
                }

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
                    
                    context.Products.AddRange(products);
                    context.SaveChanges();
                }

                if (!context.Roles.Any(r => r.Name == "admin"))
                {
                    var role = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "admin" };
                    roleManager.CreateAsync(role).Wait();
                }
                
                if (!context.Roles.Any(r => r.Name == "user"))
                {
                    var role = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "user" };
                    roleManager.CreateAsync(role).Wait();
                }

                if (!context.Users.Any())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    
                    var admin = new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        PhoneNumber = "+380111111111",
                        Age = 20
                    };
                    var user = new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "user@gmail.com",
                        UserName = "user",
                        PhoneNumber = "+380222222222",
                        Age = 20
                    };
                    var resultAdmin = userManager.CreateAsync(admin, "qwerty").Result;
                    if (resultAdmin.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "admin").Wait();
                    }
                    var resultUser = userManager.CreateAsync(user, "qwerty").Result;
                    if (resultUser.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "user").Wait();
                    }
                }
            }
        }
    }
}