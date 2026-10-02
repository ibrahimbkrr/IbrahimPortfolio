using IbrahimPortfolio.Dto.CertificateDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CertificateController : AdminBaseController
    {
        private readonly ICertificateApiService _certificateApiService;

        public CertificateController(
            ICertificateApiService certificateApiService)
        {
            _certificateApiService = certificateApiService;
        }

        public async Task<IActionResult> Index()
        {
            var certificates =
                await _certificateApiService.GetAllAsync();

            return View(certificates);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateCertificateDto createCertificateDto)
        {
            if (!ModelState.IsValid) return View(createCertificateDto);

            var result =
                await _certificateApiService.CreateAsync(createCertificateDto);

            if (!result)
            {
                ModelState.AddModelError("", "Sertifika eklenemedi.");
                return View(createCertificateDto);
            }

            TempData["SuccessMessage"] =
                "Sertifika başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var certificate =
                await _certificateApiService.GetByIdAsync(id);

            if (certificate == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var model = new UpdateCertificateDto
            {
                Id = certificate.Id,
                Name = certificate.Name,
                Issuer = certificate.Issuer,
                IssueDate = certificate.IssueDate,
                ExpirationDate = certificate.ExpirationDate,
                CredentialId = certificate.CredentialId,
                CredentialUrl = certificate.CredentialUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateCertificateDto updateCertificateDto)
        {
            if (!ModelState.IsValid) return View(updateCertificateDto);

            var result =
                await _certificateApiService.UpdateAsync(updateCertificateDto);

            if (!result)
            {
                ModelState.AddModelError("", "Sertifika güncellenemedi.");
                return View(updateCertificateDto);
            }

            TempData["SuccessMessage"] =
                "Sertifika başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _certificateApiService.DeleteAsync(id);

            if (!result) { TempData["ErrorMessage"] = "İşlem tamamlanamadı. Kayıt silinmiş olabilir veya API erişilemiyor."; return RedirectToAction("Index"); }

            TempData["SuccessMessage"] =
                "Sertifika başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
