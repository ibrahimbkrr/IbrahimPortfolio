using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.ExperienceDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class ExperienceManager : IExperienceService
    {
        private readonly IGenericRepository<Experience> _experienceRepository;
        private readonly IMapper _mapper;

        public ExperienceManager(
            IGenericRepository<Experience> experienceRepository,
            IMapper mapper)
        {
            _experienceRepository = experienceRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultExperienceDto>> GetAllAsync()
        {
            var experiences = await _experienceRepository.GetAllAsync();

            return _mapper.Map<List<ResultExperienceDto>>(experiences);
        }

        public async Task<ResultExperienceDto?> GetByIdAsync(int id)
        {
            var experience = await _experienceRepository.GetByIdAsync(id);

            if (experience == null)
                return null;

            return _mapper.Map<ResultExperienceDto>(experience);
        }

        public async Task CreateAsync(CreateExperienceDto createExperienceDto)
        {
            var experience = _mapper.Map<Experience>(createExperienceDto);

            await _experienceRepository.CreateAsync(experience);
            await _experienceRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateExperienceDto updateExperienceDto)
        {
            var experience =
                await _experienceRepository.GetByIdAsync(updateExperienceDto.Id);

            if (experience == null)
                return false;

            _mapper.Map(updateExperienceDto, experience);

            _experienceRepository.Update(experience);
            await _experienceRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var experience = await _experienceRepository.GetByIdAsync(id);

            if (experience == null)
                return false;

            _experienceRepository.Delete(experience);
            await _experienceRepository.SaveChangesAsync();

            return true;
        }
    }
}