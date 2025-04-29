using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_project.Models;

namespace mvc_project.Repositories.Products
{
    public interface IProductRepository
    {
        Task<bool> CreateAsync(Product model);
        Task<bool> UpdateAsync(Product model);
        Task<bool> DeleteAsync(string id);
        Task<Product?> GetByIdAsync(string id);
        Task<List<Product>> GetAllAsync();
        Task<List<SelectListItem>> GetCategoriesSelectListAsync();
    }
}