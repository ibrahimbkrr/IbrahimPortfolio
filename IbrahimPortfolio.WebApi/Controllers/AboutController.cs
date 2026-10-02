using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.AboutDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var about = await _aboutService.GetAllAsync();

            return Ok(about);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var about = await _aboutService.GetByIdAsync(id);

            if (about == null)
                return NotFound();

            return Ok(about);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAboutDto createAboutDto)
        {
            await _aboutService.CreateAsync(createAboutDto);

            return Ok("Hakkımda bilgisi başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAboutDto updateAboutDto)
        {
            var result = await _aboutService.UpdateAsync(updateAboutDto);

            if (!result)
                return NotFound("Hakkımda bilgisi bulunamadı.");

            return Ok("Hakkımda bilgisi başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _aboutService.DeleteAsync(id);

            if (!result)
                return NotFound("Hakkımda bilgisi bulunamadı.");

            return Ok("Hakkımda bilgisi başarıyla silindi.");
        }
    }
}