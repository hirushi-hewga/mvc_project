using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_project.Models;

namespace mvc_project.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<bool> CreateAsync(Category model);
        Task<bool> UpdateAsync(Category model);
        Task<bool> DeleteAsync(string id);
        Task<Category?> GetByIdAsync(string id);
        Task<List<Category>> GetAllAsync();
    }
}