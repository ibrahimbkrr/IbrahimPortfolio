using IbrahimPortfolio.Dto.MessageDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface IMessageApiService
    {
        Task<List<ResultMessageDto>> GetAllAsync();
        Task<ResultMessageDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateMessageDto createMessageDto);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
