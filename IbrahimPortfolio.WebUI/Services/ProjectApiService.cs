using IbrahimPortfolio.Dto.ProjectDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class ProjectApiService : IProjectApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public ProjectApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultProjectDto>> GetAllAsync()
        {
            var projects = await _requests.GetAsync<List<ResultProjectDto>>(_httpClient, "api/Projects");

            return projects ?? new List<ResultProjectDto>();
        }

        public async Task<ResultProjectDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultProjectDto>(_httpClient, $"api/Projects/{id}");
        }

        public async Task<bool> CreateAsync(CreateProjectDto createProjectDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/Projects",
                createProjectDto);
        }

        public async Task<bool> UpdateAsync(UpdateProjectDto updateProjectDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/Projects",
                updateProjectDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/Projects/{id}");
        }
    }
}