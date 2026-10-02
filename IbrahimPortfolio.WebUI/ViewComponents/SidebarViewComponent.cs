using IbrahimPortfolio.WebUI.Models;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IAboutApiService _aboutApiService;

        public SidebarViewComponent(IAboutApiService aboutApiService)
        {
            _aboutApiService = aboutApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var abouts = await _aboutApiService.GetAllAsync();

            var model = new SidebarViewModel
            {
                About = abouts.FirstOrDefault()
            };

            return View(model);
        }
    }
}