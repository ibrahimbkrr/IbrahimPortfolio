using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.ProjectDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class ProjectManager : IProjectService
    {
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IMapper _mapper;

        public ProjectManager(
            IGenericRepository<Project> projectRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();

            return _mapper.Map<List<ResultProjectDto>>(projects);
        }

        public async Task<ResultProjectDto?> GetByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return null;

            return _mapper.Map<ResultProjectDto>(project);
        }

        public async Task CreateAsync(CreateProjectDto createProjectDto)
        {
            var project = _mapper.Map<Project>(createProjectDto);

            await _projectRepository.CreateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateProjectDto updateProjectDto)
        {
            var project = await _projectRepository.GetByIdAsync(updateProjectDto.Id);

            if (project == null)
                return false;

            _mapper.Map(updateProjectDto, project);

            _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return false;

            _projectRepository.Delete(project);
            await _projectRepository.SaveChangesAsync();

            return true;
        }
    }
}