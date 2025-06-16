using Microsoft.AspNetCore.Mvc;
using mvc_project.Models;
using mvc_project.Repositories.Promocodes;
using mvc_project.Services.PromoCode;
using mvc_project.Validators;

namespace mvc_project.Controllers
{
    public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService _promoCodeService;
        private readonly IPromocodeRepository _promocodeRepository;

        public PromoCodeController(IPromoCodeService promoCodeService ,IPromocodeRepository promocodeRepository)
        {
            _promoCodeService = promoCodeService;
            _promocodeRepository = promocodeRepository;
        }
        
        public IActionResult Index()
        {
            return View(_promocodeRepository.GetAll());
        }

        public async Task<IActionResult> DeleteAsync(string id)
        {
            var promocode = await _promocodeRepository.FindByIdAsync(id);

            if (promocode == null)
                return NotFound();
            
            return View(promocode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Promocode model)
        {
            if (model.Id == null)
                return NotFound();
            
            await _promocodeRepository.DeleteAsync(model.Id);

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> EditAsync(string id)
        {
            var promocode = await _promocodeRepository.FindByIdAsync(id);
            
            if (promocode == null)
                return NotFound();
            
            var viewModel = new PromocodeCreateVM
            {
                Promocode = promocode,
                IsEdit = true
            };

            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(PromocodeCreateVM viewModel)
        {
            var validator = new PromocodeValidator();
            var result = await validator.ValidateAsync(viewModel.Promocode);
            
            if (!result.IsValid)
            {
                viewModel.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                viewModel.IsEdit = true;
                return View("Create", viewModel);
            }
        
            await _promocodeRepository.UpdateAsync(viewModel.Promocode);
        
            return RedirectToAction("Index");
        }
        
        public IActionResult Create()
        {
            return View(new PromocodeCreateVM());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PromocodeCreateVM viewModel)
        {
            var validator = new PromocodeValidator();
            var result = await validator.ValidateAsync(viewModel.Promocode);
            
            if (!result.IsValid)
            {
                viewModel.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View(viewModel);
            }
            
            await _promocodeRepository.CreateAsync(viewModel.Promocode);

            return RedirectToAction("Index");
        }
    }
}