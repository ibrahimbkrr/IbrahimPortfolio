using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.MessageDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IMapper _mapper;

        public MessageManager(
            IGenericRepository<Message> messageRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultMessageDto>> GetAllAsync()
        {
            var messages = await _messageRepository.GetAllAsync();
            return _mapper.Map<List<ResultMessageDto>>(messages);
        }

        public async Task<ResultMessageDto?> GetByIdAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);

            if (message == null)
                return null;

            return _mapper.Map<ResultMessageDto>(message);
        }

        public async Task CreateAsync(CreateMessageDto createMessageDto)
        {
            var message = _mapper.Map<Message>(createMessageDto);

            message.CreatedAt = DateTime.Now;
            message.IsRead = false;

            await _messageRepository.CreateAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);

            if (message == null)
                return false;

            message.IsRead = true;

            _messageRepository.Update(message);
            await _messageRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);

            if (message == null)
                return false;

            _messageRepository.Delete(message);
            await _messageRepository.SaveChangesAsync();

            return true;
        }
    }
}
