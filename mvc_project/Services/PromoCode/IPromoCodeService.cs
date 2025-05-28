using mvc_project.Models;

namespace mvc_project.Services.PromoCode
{
    public interface IPromoCodeService
    {
        IEnumerable<Promocode> GetAll();
        Promocode? FindById(string? id);
        void SetPromoCode(string promoCodeId);
        Promocode? GetPromoCode();
    }
}