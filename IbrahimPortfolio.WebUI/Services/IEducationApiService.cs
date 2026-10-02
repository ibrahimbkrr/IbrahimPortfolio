using IbrahimPortfolio.Dto.EducationDtos;

namespace IbrahimPortfolio.WebUI.Services
{
    public interface IEducationApiService
    {
        Task<List<ResultEducationDto>> GetAllAsync();
        Task<ResultEducationDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateEducationDto createEducationDto);
        Task<bool> UpdateAsync(UpdateEducationDto updateEducationDto);
        Task<bool> DeleteAsync(int id);
    }
}