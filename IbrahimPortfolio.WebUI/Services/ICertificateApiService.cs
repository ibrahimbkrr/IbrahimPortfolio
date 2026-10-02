using IbrahimPortfolio.Dto.CertificateDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface ICertificateApiService
    {
        Task<List<ResultCertificateDto>> GetAllAsync();
        Task<ResultCertificateDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCertificateDto createCertificateDto);
        Task<bool> UpdateAsync(UpdateCertificateDto updateCertificateDto);
        Task<bool> DeleteAsync(int id);
    }
}