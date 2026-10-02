using IbrahimPortfolio.Dto.SkillDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface ISkillApiService
    {
        Task<List<ResultSkillDto>> GetAllAsync();
        Task<ResultSkillDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateSkillDto createSkillDto);
        Task<bool> UpdateAsync(UpdateSkillDto updateSkillDto);
        Task<bool> DeleteAsync(int id);
    }
}