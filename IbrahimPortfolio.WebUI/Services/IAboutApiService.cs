using IbrahimPortfolio.Dto.AboutDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface IAboutApiService
    {
        Task<List<ResultAboutDto>> GetAllAsync();

        Task<ResultAboutDto?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(UpdateAboutDto updateAboutDto);
    }
}