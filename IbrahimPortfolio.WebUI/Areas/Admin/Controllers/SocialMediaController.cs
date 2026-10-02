using IbrahimPortfolio.Dto.SocialMediaDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialMediaController : AdminBaseController
    {
        private readonly ISocialMediaApiService _socialMediaApiService;

        public SocialMediaController(
            ISocialMediaApiService socialMediaApiService)
        {
            _socialMediaApiService = socialMediaApiService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _socialMediaApiService.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateSocialMediaDto createSocialMediaDto)
        {
            if (!ModelState.IsValid) return View(createSocialMediaDto);

            var result =
                await _socialMediaApiService.CreateAsync(createSocialMediaDto);

            if (!result)
            {
                ModelState.AddModelError("", "Sosyal medya hesabı eklenemedi.");
                return View(createSocialMediaDto);
            }

            TempData["SuccessMessage"] =
                "Sosyal medya hesabı başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value =
                await _socialMediaApiService.GetByIdAsync(id);

            if (value == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var model = new UpdateSocialMediaDto
            {
                Id = value.Id,
                Name = value.Name,
                Url = value.Url,
                Icon = value.Icon,
                DisplayOrder = value.DisplayOrder,
                IsActive = value.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateSocialMediaDto updateSocialMediaDto)
        {
            if (!ModelState.IsValid) return View(updateSocialMediaDto);

            var result =
                await _socialMediaApiService.UpdateAsync(updateSocialMediaDto);

            if (!result)
            {
                ModelState.AddModelError("", "Sosyal medya hesabı güncellenemedi.");
                return View(updateSocialMediaDto);
            }

            TempData["SuccessMessage"] =
                "Sosyal medya hesabı başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _socialMediaApiService.DeleteAsync(id);

            if (!result) { TempData["ErrorMessage"] = "İşlem tamamlanamadı. Kayıt silinmiş olabilir veya API erişilemiyor."; return RedirectToAction("Index"); }

            TempData["SuccessMessage"] =
                "Sosyal medya hesabı başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
