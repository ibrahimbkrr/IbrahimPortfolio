using IbrahimPortfolio.Dto.SocialMediaDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface ISocialMediaService
    {
        Task<List<ResultSocialMediaDto>> GetAllAsync();
        Task<ResultSocialMediaDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateSocialMediaDto createSocialMediaDto);
        Task<bool> UpdateAsync(UpdateSocialMediaDto updateSocialMediaDto);
        Task<bool> DeleteAsync(int id);
    }
}