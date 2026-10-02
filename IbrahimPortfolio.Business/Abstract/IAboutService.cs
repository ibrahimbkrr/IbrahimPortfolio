using IbrahimPortfolio.Dto.AboutDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface IAboutService
    {
        Task<List<ResultAboutDto>> GetAllAsync();

        Task<ResultAboutDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateAboutDto createAboutDto);

        Task<bool> UpdateAsync(UpdateAboutDto updateAboutDto);

        Task<bool> DeleteAsync(int id);
    }
}