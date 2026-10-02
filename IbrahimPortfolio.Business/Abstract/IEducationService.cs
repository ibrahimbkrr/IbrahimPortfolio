using IbrahimPortfolio.Dto.EducationDtos;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface IEducationService
    {
        Task<List<ResultEducationDto>> GetAllAsync();

        Task<ResultEducationDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateEducationDto createEducationDto);

        Task<bool> UpdateAsync(UpdateEducationDto updateEducationDto);

        Task<bool> DeleteAsync(int id);
    }
}