using mvc_project.Models;

namespace mvc_project.Repositories.Promocodes
{
    public interface IPromocodeRepository
        : IGenericRepository<Promocode, string>
    {
        IEnumerable<Promocode> GetAll();
    }
}