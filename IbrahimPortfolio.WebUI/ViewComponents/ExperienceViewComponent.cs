using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class ExperienceViewComponent : ViewComponent
    {
        private readonly IExperienceApiService _experienceApiService;

        public ExperienceViewComponent(
            IExperienceApiService experienceApiService)
        {
            _experienceApiService = experienceApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var experiences =
                await _experienceApiService.GetAllAsync();

            return View(experiences);
        }
    }
}