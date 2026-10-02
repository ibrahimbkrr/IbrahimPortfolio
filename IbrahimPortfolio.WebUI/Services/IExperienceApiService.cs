using IbrahimPortfolio.Dto.ExperienceDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface IExperienceApiService
    {
        Task<List<ResultExperienceDto>> GetAllAsync();
        Task<ResultExperienceDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateExperienceDto createExperienceDto);
        Task<bool> UpdateAsync(UpdateExperienceDto updateExperienceDto);
        Task<bool> DeleteAsync(int id);
    }
}