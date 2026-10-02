using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class SkillViewComponent : ViewComponent
    {
        private readonly ISkillApiService _skillApiService;

        public SkillViewComponent(ISkillApiService skillApiService)
        {
            _skillApiService = skillApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var skills = await _skillApiService.GetAllAsync();

            return View(skills);
        }
    }
}