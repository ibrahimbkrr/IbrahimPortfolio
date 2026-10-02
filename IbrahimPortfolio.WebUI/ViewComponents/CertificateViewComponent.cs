using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class CertificateViewComponent : ViewComponent
    {
        private readonly ICertificateApiService _certificateApiService;

        public CertificateViewComponent(ICertificateApiService certificateApiService)
        {
            _certificateApiService = certificateApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var certificates = await _certificateApiService.GetAllAsync();

            return View(certificates);
        }
    }
}
