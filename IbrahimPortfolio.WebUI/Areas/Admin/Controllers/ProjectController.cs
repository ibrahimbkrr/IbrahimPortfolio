using IbrahimPortfolio.Dto.ProjectDtos;
using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : AdminBaseController
    {
        private readonly IProjectApiService _projectApiService;

        public ProjectController(IProjectApiService projectApiService)
        {
            _projectApiService = projectApiService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectApiService.GetAllAsync();

            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateProjectDto createProjectDto)
        {
            if (!ModelState.IsValid) return View(createProjectDto);

            var result =
                await _projectApiService.CreateAsync(createProjectDto);

            if (!result)
            {
                ModelState.AddModelError("", "Proje eklenemedi.");
                return View(createProjectDto);
            }

            TempData["SuccessMessage"] =
                "Proje başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var project =
                await _projectApiService.GetByIdAsync(id);

            if (project == null)
                return StatusCode(HttpContext.RequestServices.GetRequiredService<ApiRequestState>().HasErrors ? 503 : 404);

            var model = new UpdateProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ImageUrl = project.ImageUrl,
                GithubUrl = project.GithubUrl,
                LiveUrl = project.LiveUrl,
                Technologies = project.Technologies
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateProjectDto updateProjectDto)
        {
            if (!ModelState.IsValid) return View(updateProjectDto);

            var result =
                await _projectApiService.UpdateAsync(updateProjectDto);

            if (!result)
            {
                ModelState.AddModelError("", "Proje güncellenemedi.");
                return View(updateProjectDto);
            }

            TempData["SuccessMessage"] =
                "Proje başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _projectApiService.DeleteAsync(id);

            if (!result) { TempData["ErrorMessage"] = "İşlem tamamlanamadı. Kayıt silinmiş olabilir veya API erişilemiyor."; return RedirectToAction("Index"); }

            TempData["SuccessMessage"] =
                "Proje başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
