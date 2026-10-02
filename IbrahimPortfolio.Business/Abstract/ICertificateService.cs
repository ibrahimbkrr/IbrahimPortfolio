using IbrahimPortfolio.Dto.CertificateDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface ICertificateService
    {
        Task<List<ResultCertificateDto>> GetAllAsync();
        Task<ResultCertificateDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateCertificateDto createCertificateDto);
        Task<bool> UpdateAsync(UpdateCertificateDto updateCertificateDto);
        Task<bool> DeleteAsync(int id);
    }
}