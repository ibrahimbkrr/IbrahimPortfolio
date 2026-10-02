using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.EducationDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class EducationManager : IEducationService
    {
        private readonly IGenericRepository<Education> _educationRepository;
        private readonly IMapper _mapper;

        public EducationManager(
            IGenericRepository<Education> educationRepository,
            IMapper mapper)
        {
            _educationRepository = educationRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultEducationDto>> GetAllAsync()
        {
            var educations = await _educationRepository.GetAllAsync();

            return _mapper.Map<List<ResultEducationDto>>(educations);
        }

        public async Task<ResultEducationDto?> GetByIdAsync(int id)
        {
            var education = await _educationRepository.GetByIdAsync(id);

            if (education == null)
                return null;

            return _mapper.Map<ResultEducationDto>(education);
        }

        public async Task CreateAsync(CreateEducationDto createEducationDto)
        {
            var education = _mapper.Map<Education>(createEducationDto);

            await _educationRepository.CreateAsync(education);
            await _educationRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateEducationDto updateEducationDto)
        {
            var education =
                await _educationRepository.GetByIdAsync(updateEducationDto.Id);

            if (education == null)
                return false;

            _mapper.Map(updateEducationDto, education);

            _educationRepository.Update(education);
            await _educationRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var education = await _educationRepository.GetByIdAsync(id);

            if (education == null)
                return false;

            _educationRepository.Delete(education);
            await _educationRepository.SaveChangesAsync();

            return true;
        }
    }
}