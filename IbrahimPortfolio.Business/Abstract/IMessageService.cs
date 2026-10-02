using IbrahimPortfolio.Dto.MessageDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface IMessageService
    {
        Task<List<ResultMessageDto>> GetAllAsync();
        Task<ResultMessageDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateMessageDto createMessageDto);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
