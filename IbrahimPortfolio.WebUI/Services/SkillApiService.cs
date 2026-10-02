using IbrahimPortfolio.Dto.SkillDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class SkillApiService : ISkillApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public SkillApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultSkillDto>> GetAllAsync()
        {
            var skills = await _requests.GetAsync<List<ResultSkillDto>>(_httpClient, "api/Skill");

            return skills ?? new List<ResultSkillDto>();
        }

        public async Task<ResultSkillDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultSkillDto>(_httpClient, $"api/Skill/{id}");
        }

        public async Task<bool> CreateAsync(CreateSkillDto createSkillDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/Skill",
                createSkillDto);
        }

        public async Task<bool> UpdateAsync(UpdateSkillDto updateSkillDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/Skill",
                updateSkillDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/Skill/{id}");
        }
    }
}