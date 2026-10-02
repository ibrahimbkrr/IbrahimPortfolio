using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.SocialMediaDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : ControllerBase
    {
        private readonly ISocialMediaService _socialMediaService;

        public SocialMediaController(ISocialMediaService socialMediaService)
        {
            _socialMediaService = socialMediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var socialMedias = await _socialMediaService.GetAllAsync();
            return Ok(socialMedias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var socialMedia = await _socialMediaService.GetByIdAsync(id);

            if (socialMedia == null)
                return NotFound();

            return Ok(socialMedia);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSocialMediaDto createSocialMediaDto)
        {
            await _socialMediaService.CreateAsync(createSocialMediaDto);

            return Ok("Sosyal medya hesabı başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateSocialMediaDto updateSocialMediaDto)
        {
            var result = await _socialMediaService.UpdateAsync(updateSocialMediaDto);

            if (!result)
                return NotFound("Sosyal medya hesabı bulunamadı.");

            return Ok("Sosyal medya hesabı başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _socialMediaService.DeleteAsync(id);

            if (!result)
                return NotFound("Sosyal medya hesabı bulunamadı.");

            return Ok("Sosyal medya hesabı başarıyla silindi.");
        }
    }
}