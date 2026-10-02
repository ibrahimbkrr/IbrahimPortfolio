using IbrahimPortfolio.Dto.CertificateDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class CertificateApiService : ICertificateApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public CertificateApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultCertificateDto>> GetAllAsync()
        {
            var certificates = await _requests.GetAsync<List<ResultCertificateDto>>(_httpClient, "api/Certificate");

            return certificates ?? new List<ResultCertificateDto>();
        }

        public async Task<ResultCertificateDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultCertificateDto>(_httpClient, $"api/Certificate/{id}");
        }

        public async Task<bool> CreateAsync(CreateCertificateDto createCertificateDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/Certificate",
                createCertificateDto);
        }

        public async Task<bool> UpdateAsync(UpdateCertificateDto updateCertificateDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                "api/Certificate",
                updateCertificateDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/Certificate/{id}");
        }
    }
}