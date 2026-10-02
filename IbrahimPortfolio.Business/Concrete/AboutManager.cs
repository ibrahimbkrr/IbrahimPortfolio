using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.AboutDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class AboutManager : IAboutService
    {
        private readonly IGenericRepository<About> _aboutRepository;
        private readonly IMapper _mapper;

        public AboutManager(
            IGenericRepository<About> aboutRepository,
            IMapper mapper)
        {
            _aboutRepository = aboutRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultAboutDto>> GetAllAsync()
        {
            var abouts = await _aboutRepository.GetAllAsync();

            return _mapper.Map<List<ResultAboutDto>>(abouts);
        }

        public async Task<ResultAboutDto?> GetByIdAsync(int id)
        {
            var about = await _aboutRepository.GetByIdAsync(id);

            if (about == null)
                return null;

            return _mapper.Map<ResultAboutDto>(about);
        }

        public async Task CreateAsync(CreateAboutDto createAboutDto)
        {
            var about = _mapper.Map<About>(createAboutDto);

            await _aboutRepository.CreateAsync(about);
            await _aboutRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateAboutDto updateAboutDto)
        {
            var about = await _aboutRepository.GetByIdAsync(updateAboutDto.Id);

            if (about == null)
                return false;

            _mapper.Map(updateAboutDto, about);

            _aboutRepository.Update(about);
            await _aboutRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var about = await _aboutRepository.GetByIdAsync(id);

            if (about == null)
                return false;

            _aboutRepository.Delete(about);
            await _aboutRepository.SaveChangesAsync();

            return true;
        }
    }
}