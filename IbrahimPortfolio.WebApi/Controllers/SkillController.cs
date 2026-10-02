using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.SkillDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillService.GetAllAsync();
            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);

            if (skill == null)
                return NotFound();

            return Ok(skill);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSkillDto createSkillDto)
        {
            await _skillService.CreateAsync(createSkillDto);
            return Ok("Yetenek başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateSkillDto updateSkillDto)
        {
            var result = await _skillService.UpdateAsync(updateSkillDto);

            if (!result)
                return NotFound("Yetenek bulunamadı.");

            return Ok("Yetenek başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _skillService.DeleteAsync(id);

            if (!result)
                return NotFound("Yetenek bulunamadı.");

            return Ok("Yetenek başarıyla silindi.");
        }
    }
}