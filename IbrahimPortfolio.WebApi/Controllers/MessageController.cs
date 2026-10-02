using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.Dto.MessageDtos;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _messageService.GetByIdAsync(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageDto createMessageDto)
        {
            await _messageService.CreateAsync(createMessageDto);

            return Ok("Mesaj başarıyla gönderildi.");
        }

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _messageService.MarkAsReadAsync(id);

            if (!result)
                return NotFound("Mesaj bulunamadı.");

            return Ok("Mesaj başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _messageService.DeleteAsync(id);

            if (!result)
                return NotFound("Mesaj bulunamadı.");

            return Ok("Mesaj başarıyla silindi.");
        }
    }
}
