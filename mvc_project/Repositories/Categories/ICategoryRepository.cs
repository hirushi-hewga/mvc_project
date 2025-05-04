using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_project.Models;

namespace mvc_project.Repositories.Categories
{
    public interface ICategoryRepository
        : IGenericRepository<Category, string>
    {
        IQueryable<Category> Categories { get; }
        // Task<Category?> GetByNameAsync(string name);
    }
}