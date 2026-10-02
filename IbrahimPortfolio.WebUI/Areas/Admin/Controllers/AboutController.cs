using IbrahimPortfolio.Dto.AboutDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : AdminBaseController
    {
        private readonly IAboutApiService _aboutApiService;

        public AboutController(IAboutApiService aboutApiService)
        {
            _aboutApiService = aboutApiService;
        }

        public async Task<IActionResult> Index()
        {
            var abouts = await _aboutApiService.GetAllAsync();

            return View(abouts);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var about = await _aboutApiService.GetByIdAsync(id);

            if (about == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var updateAboutDto = new UpdateAboutDto
            {
                Id = about.Id,
                FirstName = about.FirstName,
                LastName = about.LastName,
                Title = about.Title,
                Description = about.Description,
                ImageUrl = about.ImageUrl,
                PhoneNumber = about.PhoneNumber,
                Email = about.Email,
                Address = about.Address,
                CvUrl = about.CvUrl
            };

            return View(updateAboutDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateAboutDto updateAboutDto)
        {
            if (!ModelState.IsValid) return View(updateAboutDto);

            var result = await _aboutApiService.UpdateAsync(updateAboutDto);

            if (!result)
            {
                ModelState.AddModelError("", "Hakkımda bilgisi güncellenemedi.");
                return View(updateAboutDto);
            }

            TempData["SuccessMessage"] =
                "Hakkımda bilgisi başarıyla güncellendi.";

            return RedirectToAction("Index");
        }
    }
}
