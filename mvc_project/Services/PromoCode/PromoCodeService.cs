using System.Text;
using Microsoft.EntityFrameworkCore;
using mvc_project.Data;
using mvc_project.Models;
using mvc_project.Repositories.Promocodes;
using mvc_project.Services.Session;

namespace mvc_project.Services.PromoCode
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPromocodeRepository _promocodeRepository;

        public PromoCodeService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IPromocodeRepository promocodeRepository)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _promocodeRepository = promocodeRepository;
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

        public async Task<Promocode?> GetPromoCodeAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return new Promocode();
            var session = context.Session;
            var bytes = session.Get(Settings.SessionPromoCodeKey);
            if (bytes == null)
                return new Promocode();
            var promoCodeId = Encoding.ASCII.GetString(bytes).Trim('"');
            return await _promocodeRepository.FindByIdAsync(promoCodeId);
        }
    }
}