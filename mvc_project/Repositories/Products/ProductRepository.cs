using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;
using mvc_project.Repositories.Categories;

namespace mvc_project.Repositories.Products
{
    public class ProductRepository
        : GenericRepository<Product, string>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(AppDbContext context, ICategoryRepository categoryRepository) 
            : base(context)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }
        
        public IQueryable<Product> Products => GetAll().Include(p => p.Category);

        public IQueryable<Product> GetByCategoryId(string categoryId)
        {
            var products = Products.Where(p => 
                p.Category == null ? false 
                : p.Category.Id == null ? false
                : p.Category.Id == categoryId);
            
            return products;
        }

        public async Task<List<Product>> FindByCategoryIdAsync(string id)
        {
            var models = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id).ToListAsync();
            return models;
        }

        public Product? FindById(string id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            return product;
        }

        public async Task<List<SelectListItem>> GetCategoriesSelectListAsync()
        {
            var models = _categoryRepository.GetAll();
            var result = await models.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToListAsync();
            return result;
        }
    }
}