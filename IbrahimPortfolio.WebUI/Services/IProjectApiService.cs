using IbrahimPortfolio.Dto.ProjectDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface IProjectApiService
    {
        Task<List<ResultProjectDto>> GetAllAsync();
        Task<ResultProjectDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateProjectDto createProjectDto);
        Task<bool> UpdateAsync(UpdateProjectDto updateProjectDto);
        Task<bool> DeleteAsync(int id);
    }
}