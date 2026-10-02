using IbrahimPortfolio.Dto.EducationDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EducationController : AdminBaseController
    {
        private readonly IEducationApiService _educationApiService;

        public EducationController(
            IEducationApiService educationApiService)
        {
            _educationApiService = educationApiService;
        }

        public async Task<IActionResult> Index()
        {
            var educations =
                await _educationApiService.GetAllAsync();

            return View(educations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateEducationDto createEducationDto)
        {
            if (!ModelState.IsValid) return View(createEducationDto);

            var result =
                await _educationApiService.CreateAsync(createEducationDto);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Eğitim bilgisi eklenemedi.");

                return View(createEducationDto);
            }

            TempData["SuccessMessage"] =
                "Eğitim bilgisi başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var education =
                await _educationApiService.GetByIdAsync(id);

            if (education == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var model = new UpdateEducationDto
            {
                Id = education.Id,
                SchoolName = education.SchoolName,
                Department = education.Department,
                Degree = education.Degree,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                Description = education.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateEducationDto updateEducationDto)
        {
            if (!ModelState.IsValid) return View(updateEducationDto);

            var result =
                await _educationApiService.UpdateAsync(updateEducationDto);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Eğitim bilgisi güncellenemedi.");

                return View(updateEducationDto);
            }

            TempData["SuccessMessage"] =
                "Eğitim bilgisi başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _educationApiService.DeleteAsync(id);

            if (!result) { TempData["ErrorMessage"] = "İşlem tamamlanamadı. Kayıt silinmiş olabilir veya API erişilemiyor."; return RedirectToAction("Index"); }

            TempData["SuccessMessage"] =
                "Eğitim bilgisi başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
