using IbrahimPortfolio.Dto.SocialMediaDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class SocialMediaApiService : ISocialMediaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public SocialMediaApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultSocialMediaDto>> GetAllAsync()
        {
            var socialMedias = await _requests.GetAsync<List<ResultSocialMediaDto>>(_httpClient, "api/SocialMedia");

            return socialMedias ?? new List<ResultSocialMediaDto>();
        }

        public async Task<ResultSocialMediaDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultSocialMediaDto>(_httpClient, $"api/SocialMedia/{id}");
        }

        public async Task<bool> CreateAsync(CreateSocialMediaDto createSocialMediaDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/SocialMedia",
                createSocialMediaDto);
        }

        public async Task<bool> UpdateAsync(UpdateSocialMediaDto updateSocialMediaDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/SocialMedia",
                updateSocialMediaDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/SocialMedia/{id}");
        }
    }
}