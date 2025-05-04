using mvc_project.Models;

namespace mvc_project.Repositories
{
    public interface IGenericRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        Task<bool> CreateAsync(TModel model);
        Task<bool> UpdateAsync(TModel model);
        Task<bool> DeleteAsync(TId id);
        Task<TModel?> FindByIdAsync(TId id);
        IQueryable<TModel> GetAll();
    }
}
