using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_project.Models;

namespace mvc_project.Repositories.Products
{
    public interface IProductRepository
        : IGenericRepository<Product, string>
    {
        IQueryable<Product> Products { get; }
        IQueryable<Product> GetByCategoryId(string categoryId);
        Task<List<Product>> FindByCategoryIdAsync(string id);
        Product? FindById(string id);
        Task<List<SelectListItem>> GetCategoriesSelectListAsync();
    }
}