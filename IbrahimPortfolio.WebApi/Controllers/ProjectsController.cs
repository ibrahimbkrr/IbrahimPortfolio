using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.ProjectDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto createProjectDto)
        {
            await _projectService.CreateAsync(createProjectDto);

            return Ok("Proje başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProjectDto updateProjectDto)
        {
            var result = await _projectService.UpdateAsync(updateProjectDto);

            if (!result)
                return NotFound("Proje bulunamadı.");

            return Ok("Proje başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteAsync(id);

            if (!result)
                return NotFound("Proje bulunamadı.");

            return Ok("Proje başarıyla silindi.");
        }
    }
}