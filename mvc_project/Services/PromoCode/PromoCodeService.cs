using System.Text;
using mvc_project.Data;
using mvc_project.Models;
using mvc_project.Services.Session;

namespace mvc_project.Services.PromoCode
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PromoCodeService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Promocode> GetAll()
        {
            return _dbContext.Promocodes;
        }

        public Promocode? FindById(string? id)
        {
            var promoCode = _dbContext.Promocodes.FirstOrDefault(p => p.Id == id);

            return promoCode;
        }

        public void SetPromoCode(string? promoCodeId)
        {
            if (string.IsNullOrEmpty(promoCodeId))
                return;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return;
            var session = context.Session;
            session.Set(Settings.SessionPromoCodeKey, promoCodeId);
        }

        public Promocode? GetPromoCode()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return new Promocode();
            var session = context.Session;
            var bytes = session.Get(Settings.SessionPromoCodeKey);
            if (bytes == null)
                return new Promocode();
            var promoCodeId = Encoding.ASCII.GetString(bytes).Trim('"');
            return FindById(promoCodeId);
        }
    }
}