using IbrahimPortfolio.Dto.ExperienceDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class ExperienceApiService : IExperienceApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public ExperienceApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultExperienceDto>> GetAllAsync()
        {
            var experiences = await _requests.GetAsync<List<ResultExperienceDto>>(_httpClient, "api/Experience");

            return experiences ?? new List<ResultExperienceDto>();
        }

        public async Task<ResultExperienceDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultExperienceDto>(_httpClient, $"api/Experience/{id}");
        }

        public async Task<bool> CreateAsync(CreateExperienceDto createExperienceDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/Experience",
                createExperienceDto
            );
        }

        public async Task<bool> UpdateAsync(UpdateExperienceDto updateExperienceDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/Experience",
                updateExperienceDto
            );
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/Experience/{id}"
            );
        }
    }
}