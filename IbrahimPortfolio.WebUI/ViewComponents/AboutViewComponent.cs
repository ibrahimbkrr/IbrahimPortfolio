using IbrahimPortfolio.WebUI.Models;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly IAboutApiService _aboutApiService;
        private readonly ISocialMediaApiService _socialMediaApiService;

        public AboutViewComponent(
            IAboutApiService aboutApiService,
            ISocialMediaApiService socialMediaApiService)
        {
            _aboutApiService = aboutApiService;
            _socialMediaApiService = socialMediaApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var abouts = await _aboutApiService.GetAllAsync();
            var socialMedias = await _socialMediaApiService.GetAllAsync();

            var model = new AboutViewModel
            {
                About = abouts.FirstOrDefault(),

                SocialMedias = socialMedias
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
            };

            return View(model);
        }
    }
}