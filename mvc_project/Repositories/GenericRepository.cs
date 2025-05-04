using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;

namespace mvc_project.Repositories
{
    public class GenericRepository<TModel, TId>
        : IGenericRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TModel model)
        {
            await _context.Set<TModel>().AddAsync(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(TModel model)
        {
            _context.Set<TModel>().Update(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            var model = await _context.Set<TModel>().FirstOrDefaultAsync();
            _context.Set<TModel>().Remove(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<TModel?> FindByIdAsync(TId id)
        {
            var model = await _context
                .Set<TModel>()
                .FirstOrDefaultAsync(x => x.Id == null ? false : x.Id.Equals(id));
            return model;
        }

        public IQueryable<TModel> GetAll()
        {
            return _context.Set<TModel>();
        }
    }
}
