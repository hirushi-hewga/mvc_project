using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;
using mvc_project.Repositories.Categories;

namespace mvc_project.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(AppDbContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }
        
        public async Task<bool> CreateAsync(Product model)
        {
            await _context.Products.AddAsync(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Product model)
        {
            _context.Products.Update(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var model = await GetByIdAsync(id);

            if (model == null)
                return false;
            
            _context.Products.Remove(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var model = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return model;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var models = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
            return models;
        }

        public async Task<List<SelectListItem>> GetCategoriesSelectListAsync()
        {
            var models = await _categoryRepository.GetAllAsync();
            var result = models.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            return result;
        }
    }
}