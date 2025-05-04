using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_project.Models;

namespace mvc_project.Repositories.Products
{
    public interface IProductRepository
        : IGenericRepository<Product, string>
    {
        IQueryable<Product> Products { get; }
        Task<List<Product>> GetByCategoryIdAsync(string id);
        Task<List<SelectListItem>> GetCategoriesSelectListAsync();
    }
}