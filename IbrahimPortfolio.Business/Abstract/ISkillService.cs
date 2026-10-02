using IbrahimPortfolio.Dto.SkillDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface ISkillService
    {
        Task<List<ResultSkillDto>> GetAllAsync();
        Task<ResultSkillDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateSkillDto createSkillDto);
        Task<bool> UpdateAsync(UpdateSkillDto updateSkillDto);
        Task<bool> DeleteAsync(int id);
    }
}