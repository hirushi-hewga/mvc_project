using mvc_project.Models;

namespace mvc_project.Services.PromoCode
{
    public interface IPromoCodeService
    {
        void SetPromoCode(string promoCodeId);
        Task<Promocode?> GetPromoCodeAsync();
    }
}