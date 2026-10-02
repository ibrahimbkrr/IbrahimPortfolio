using IbrahimPortfolio.Dto.SkillDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SkillController : AdminBaseController
    {
        private readonly ISkillApiService _skillApiService;

        public SkillController(ISkillApiService skillApiService)
        {
            _skillApiService = skillApiService;
        }

        public async Task<IActionResult> Index()
        {
            var skills = await _skillApiService.GetAllAsync();

            return View(skills);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateSkillDto createSkillDto)
        {
            if (!ModelState.IsValid) return View(createSkillDto);

            var result =
                await _skillApiService.CreateAsync(createSkillDto);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Yetenek eklenemedi.");

                return View(createSkillDto);
            }

            TempData["SuccessMessage"] =
                "Yetenek başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var skill =
                await _skillApiService.GetByIdAsync(id);

            if (skill == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var model = new UpdateSkillDto
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.Level
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateSkillDto updateSkillDto)
        {
            if (!ModelState.IsValid) return View(updateSkillDto);

            var result =
                await _skillApiService.UpdateAsync(updateSkillDto);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Yetenek güncellenemedi.");

                return View(updateSkillDto);
            }

            TempData["SuccessMessage"] =
                "Yetenek başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _skillApiService.DeleteAsync(id);

            if (!result) { TempData["ErrorMessage"] = "İşlem tamamlanamadı. Kayıt silinmiş olabilir veya API erişilemiyor."; return RedirectToAction("Index"); }

            TempData["SuccessMessage"] =
                "Yetenek başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
