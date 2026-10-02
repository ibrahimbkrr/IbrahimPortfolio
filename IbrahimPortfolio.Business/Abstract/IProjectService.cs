using IbrahimPortfolio.Dto.ProjectDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface IProjectService
    {
        Task<List<ResultProjectDto>> GetAllAsync();

        Task<ResultProjectDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateProjectDto createProjectDto);

        Task<bool> UpdateAsync(UpdateProjectDto updateProjectDto);

        Task<bool> DeleteAsync(int id);
    }
}