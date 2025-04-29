using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;

namespace mvc_project.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> CreateAsync(Category model)
        {
            await _context.Categories.AddAsync(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Category model)
        {
            _context.Categories.Update(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var model = await GetByIdAsync(id);

            if (model == null)
                return false;
            
            _context.Categories.Remove(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Category?> GetByIdAsync(string id)
        {
            var model = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return model;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var models = await _context.Categories.ToListAsync();
            return models;
        }
    }
}