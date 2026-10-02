using IbrahimPortfolio.Dto.ExperienceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbrahimPortfolio.Business.Abstract
{
    public interface IExperienceService
    {
        Task<List<ResultExperienceDto>> GetAllAsync();
        Task<ResultExperienceDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateExperienceDto createExperienceDto);
        Task<bool> UpdateAsync(UpdateExperienceDto updateExperienceDto);
        Task<bool> DeleteAsync(int id);
    }
}
