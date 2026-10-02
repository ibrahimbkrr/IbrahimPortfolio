using IbrahimPortfolio.Dto.ExperienceDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExperienceController : AdminBaseController
    {
        private readonly IExperienceApiService _experienceApiService;

        public ExperienceController(
            IExperienceApiService experienceApiService)
        {
            _experienceApiService = experienceApiService;
        }

        public async Task<IActionResult> Index()
        {
            var experiences =
                await _experienceApiService.GetAllAsync();

            return View(experiences);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateExperienceDto createExperienceDto)
        {
            if (!ModelState.IsValid) return View(createExperienceDto);

            var result =
                await _experienceApiService.CreateAsync(createExperienceDto);

            if (!result)
            {
                ModelState.AddModelError("", "Deneyim eklenemedi.");
                return View(createExperienceDto);
            }

            TempData["SuccessMessage"] =
                "Deneyim başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var experience =
                await _experienceApiService.GetByIdAsync(id);

            if (experience == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var model = new UpdateExperienceDto
            {
                Id = experience.Id,
                CompanyName = experience.CompanyName,
                Position = experience.Position,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                Description = experience.Description,
                IsCurrent = experience.IsCurrent
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateExperienceDto updateExperienceDto)
        {
            if (!ModelState.IsValid) return View(updateExperienceDto);

            var result =
                await _experienceApiService.UpdateAsync(updateExperienceDto);

            if (!result)
            {
                ModelState.AddModelError("", "Deneyim güncellenemedi.");
                return View(updateExperienceDto);
            }

            TempData["SuccessMessage"] =
                "Deneyim başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _experienceApiService.DeleteAsync(id);

            if (!result) { TempData["ErrorMessage"] = "İşlem tamamlanamadı. Kayıt silinmiş olabilir veya API erişilemiyor."; return RedirectToAction("Index"); }

            TempData["SuccessMessage"] =
                "Deneyim başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
