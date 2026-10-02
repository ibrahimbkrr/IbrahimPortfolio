using IbrahimPortfolio.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class DashboardController : AdminBaseController
    {
        private readonly IProjectApiService _projectApiService;
        private readonly ISkillApiService _skillApiService;
        private readonly ICertificateApiService _certificateApiService;
        private readonly IMessageApiService _messageApiService;

        public DashboardController(
            IProjectApiService projectApiService,
            ISkillApiService skillApiService,
            ICertificateApiService certificateApiService,
            IMessageApiService messageApiService)
        {
            _projectApiService = projectApiService;
            _skillApiService = skillApiService;
            _certificateApiService = certificateApiService;
            _messageApiService = messageApiService;
        }


        public async Task<IActionResult> Index()
        {
            var projectsTask = _projectApiService.GetAllAsync();

            var skillsTask = _skillApiService.GetAllAsync();

            var certificatesTask = _certificateApiService.GetAllAsync();

            var messagesTask = _messageApiService.GetAllAsync();


            await Task.WhenAll(projectsTask, skillsTask, certificatesTask, messagesTask);
            var projects = await projectsTask;
            var skills = await skillsTask;
            var certificates = await certificatesTask;
            var messages = await messagesTask;
            var requests = HttpContext.RequestServices.GetRequiredService<ApiRequestState>();
            ViewBag.ProjectCount = requests.Failed("api/Projects") ? "—" : projects.Count.ToString();

            ViewBag.SkillCount = requests.Failed("api/Skill") ? "—" : skills.Count.ToString();

            ViewBag.CertificateCount = requests.Failed("api/Certificate") ? "—" : certificates.Count.ToString();

            ViewBag.MessageCount =
                requests.Failed("api/Message") ? "—" : messages.Count(x => !x.IsRead).ToString();


            ViewBag.LastMessages =
                messages
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToList();


            return View();
        }
    }
}

