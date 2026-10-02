using IbrahimPortfolio.Dto.MessageDtos;
using System.Net.Http.Json;

namespace IbrahimPortfolio.WebUI.Services
{
    public class MessageApiService : IMessageApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRequestState _requests;

        public MessageApiService(HttpClient httpClient, ApiRequestState requests)
        {
            _httpClient = httpClient;
            _requests = requests;
        }

        public async Task<List<ResultMessageDto>> GetAllAsync()
        {
            var messages = await _requests.GetAsync<List<ResultMessageDto>>(_httpClient, "api/Message");

            return messages ?? new List<ResultMessageDto>();
        }

        public async Task<ResultMessageDto?> GetByIdAsync(int id)
        {
            return await _requests.GetAsync<ResultMessageDto>(_httpClient, $"api/Message/{id}");
        }

        public async Task<bool> CreateAsync(CreateMessageDto createMessageDto)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Post, 
                "api/Message",
                createMessageDto);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Put, 
                $"api/Message/{id}/read");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _requests.SendAsync(_httpClient, HttpMethod.Delete, 
                $"api/Message/{id}");
        }
    }
}
