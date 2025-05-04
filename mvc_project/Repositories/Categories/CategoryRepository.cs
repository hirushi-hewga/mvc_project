using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;

namespace mvc_project.Repositories.Categories
{
    public class CategoryRepository 
        : GenericRepository<Category, string>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) 
            : base(context)
        {
            _context = context;
        }

        // public async Task<Category?> GetByNameAsync(string name)
        // {
        //     var model = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        //     return model;
        // }
        public IQueryable<Category> Categories => GetAll().Include(c => c.Products);
    }
}