using mvc_project.Data;
using mvc_project.Models;
using mvc_project.Repositories.Categories;

namespace mvc_project.Repositories.Promocodes
{
    public class PromocodeRepository
        : GenericRepository<Promocode, string>, IPromocodeRepository
    {
        private readonly AppDbContext _context;

        public PromocodeRepository(AppDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<Promocode> GetAll()
        {
            return _context.Promocodes;
        }
    }
}