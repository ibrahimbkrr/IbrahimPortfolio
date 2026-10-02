using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.ViewComponents
{
    public class ProjectViewComponent : ViewComponent
    {
        private readonly IProjectApiService _projectApiService;

        public ProjectViewComponent(IProjectApiService projectApiService)
        {
            _projectApiService = projectApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var projects = await _projectApiService.GetAllAsync();

            return View(projects);
        }
    }
}
