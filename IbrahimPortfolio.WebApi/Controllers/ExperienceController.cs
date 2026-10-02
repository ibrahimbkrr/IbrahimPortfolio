using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.ExperienceDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _experienceService;

        public ExperienceController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var experiences = await _experienceService.GetAllAsync();

            return Ok(experiences);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var experience = await _experienceService.GetByIdAsync(id);

            if (experience == null)
                return NotFound();

            return Ok(experience);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExperienceDto createExperienceDto)
        {
            await _experienceService.CreateAsync(createExperienceDto);

            return Ok("Deneyim başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateExperienceDto updateExperienceDto)
        {
            var result = await _experienceService.UpdateAsync(updateExperienceDto);

            if (!result)
                return NotFound("Deneyim bulunamadı.");

            return Ok("Deneyim başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _experienceService.DeleteAsync(id);

            if (!result)
                return NotFound("Deneyim bulunamadı.");

            return Ok("Deneyim başarıyla silindi.");
        }
    }
}