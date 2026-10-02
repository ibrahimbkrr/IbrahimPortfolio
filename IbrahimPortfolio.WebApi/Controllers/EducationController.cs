using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.EducationDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var educations = await _educationService.GetAllAsync();

            return Ok(educations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var education = await _educationService.GetByIdAsync(id);

            if (education == null)
                return NotFound();

            return Ok(education);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEducationDto createEducationDto)
        {
            await _educationService.CreateAsync(createEducationDto);

            return Ok("Eğitim bilgisi başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateEducationDto updateEducationDto)
        {
            var result = await _educationService.UpdateAsync(updateEducationDto);

            if (!result)
                return NotFound("Eğitim bilgisi bulunamadı.");

            return Ok("Eğitim bilgisi başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _educationService.DeleteAsync(id);

            if (!result)
                return NotFound("Eğitim bilgisi bulunamadı.");

            return Ok("Eğitim bilgisi başarıyla silindi.");
        }
    }
}