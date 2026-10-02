using IbrahimPortfolio.Dto.SocialMediaDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface ISocialMediaApiService
    {
        Task<List<ResultSocialMediaDto>> GetAllAsync();
        Task<ResultSocialMediaDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSocialMediaDto createSocialMediaDto);
        Task<bool> UpdateAsync(UpdateSocialMediaDto updateSocialMediaDto);
        Task<bool> DeleteAsync(int id);
    }
}