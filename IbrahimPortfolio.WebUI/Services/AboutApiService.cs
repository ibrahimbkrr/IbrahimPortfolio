using IbrahimPortfolio.Dto.AboutDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class AboutApiService : IAboutApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public AboutApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultAboutDto>> GetAllAsync()
        {
            var abouts = await _requests.GetAsync<List<ResultAboutDto>>(_httpClient, "api/About");

            return abouts ?? new List<ResultAboutDto>();
        }

        public async Task<ResultAboutDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultAboutDto>(_httpClient, $"api/About/{id}");
        }

        public async Task<bool> UpdateAsync(UpdateAboutDto updateAboutDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/About",
                updateAboutDto
            );
        }
    }
}