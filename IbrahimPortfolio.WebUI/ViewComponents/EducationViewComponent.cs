using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class EducationViewComponent : ViewComponent
    {
        private readonly IEducationApiService _educationApiService;

        public EducationViewComponent(IEducationApiService educationApiService)
        {
            _educationApiService = educationApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var educations = await _educationApiService.GetAllAsync();

            return View(educations);
        }
    }
}
