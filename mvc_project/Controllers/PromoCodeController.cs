using Microsoft.AspNetCore.Mvc;
using mvc_project.Services.PromoCode;

namespace mvc_project.Controllers
{
    public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }
        
        public IActionResult Index()
        {
            return View(_promoCodeService.GetAll());
        }
    }
}