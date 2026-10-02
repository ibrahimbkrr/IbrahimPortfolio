using IbrahimPortfolio.Dto.EducationDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class EducationApiService : IEducationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public EducationApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultEducationDto>> GetAllAsync()
        {
            var educations = await _requests.GetAsync<List<ResultEducationDto>>(_httpClient, "api/Education");

            return educations ?? new List<ResultEducationDto>();
        }

        public async Task<ResultEducationDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultEducationDto>(_httpClient, $"api/Education/{id}");
        }

        public async Task<bool> CreateAsync(CreateEducationDto createEducationDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/Education",
                createEducationDto);
        }

        public async Task<bool> UpdateAsync(UpdateEducationDto updateEducationDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/Education",
                updateEducationDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/Education/{id}");
        }
    }
}