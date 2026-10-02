using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.CertificateDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var certificates = await _certificateService.GetAllAsync();
            return Ok(certificates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var certificate = await _certificateService.GetByIdAsync(id);

            if (certificate == null)
                return NotFound();

            return Ok(certificate);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCertificateDto createCertificateDto)
        {
            await _certificateService.CreateAsync(createCertificateDto);

            return Ok("Sertifika başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCertificateDto updateCertificateDto)
        {
            var result = await _certificateService.UpdateAsync(updateCertificateDto);

            if (!result)
                return NotFound("Sertifika bulunamadı.");

            return Ok("Sertifika başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _certificateService.DeleteAsync(id);

            if (!result)
                return NotFound("Sertifika bulunamadı.");

            return Ok("Sertifika başarıyla silindi.");
        }
    }
}