using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.SkillDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class SkillManager : ISkillService
    {
        private readonly IGenericRepository<Skill> _skillRepository;
        private readonly IMapper _mapper;

        public SkillManager(
            IGenericRepository<Skill> skillRepository,
            IMapper mapper)
        {
            _skillRepository = skillRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultSkillDto>> GetAllAsync()
        {
            var skills = await _skillRepository.GetAllAsync();
            return _mapper.Map<List<ResultSkillDto>>(skills);
        }

        public async Task<ResultSkillDto?> GetByIdAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);

            if (skill == null)
                return null;

            return _mapper.Map<ResultSkillDto>(skill);
        }

        public async Task CreateAsync(CreateSkillDto createSkillDto)
        {
            var skill = _mapper.Map<Skill>(createSkillDto);

            await _skillRepository.CreateAsync(skill);
            await _skillRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateSkillDto updateSkillDto)
        {
            var skill = await _skillRepository.GetByIdAsync(updateSkillDto.Id);

            if (skill == null)
                return false;

            _mapper.Map(updateSkillDto, skill);

            _skillRepository.Update(skill);
            await _skillRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);

            if (skill == null)
                return false;

            _skillRepository.Delete(skill);
            await _skillRepository.SaveChangesAsync();

            return true;
        }
    }
}