using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.SocialMediaDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class SocialMediaManager : ISocialMediaService
    {
        private readonly IGenericRepository<SocialMedia> _socialMediaRepository;
        private readonly IMapper _mapper;

        public SocialMediaManager(
            IGenericRepository<SocialMedia> socialMediaRepository,
            IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultSocialMediaDto>> GetAllAsync()
        {
            var socialMedias = await _socialMediaRepository.GetAllAsync();
            return _mapper.Map<List<ResultSocialMediaDto>>(socialMedias);
        }

        public async Task<ResultSocialMediaDto?> GetByIdAsync(int id)
        {
            var socialMedia = await _socialMediaRepository.GetByIdAsync(id);

            if (socialMedia == null)
                return null;

            return _mapper.Map<ResultSocialMediaDto>(socialMedia);
        }

        public async Task CreateAsync(CreateSocialMediaDto createSocialMediaDto)
        {
            var socialMedia = _mapper.Map<SocialMedia>(createSocialMediaDto);

            await _socialMediaRepository.CreateAsync(socialMedia);
            await _socialMediaRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateSocialMediaDto updateSocialMediaDto)
        {
            var socialMedia =
                await _socialMediaRepository.GetByIdAsync(updateSocialMediaDto.Id);

            if (socialMedia == null)
                return false;

            _mapper.Map(updateSocialMediaDto, socialMedia);

            _socialMediaRepository.Update(socialMedia);
            await _socialMediaRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var socialMedia = await _socialMediaRepository.GetByIdAsync(id);

            if (socialMedia == null)
                return false;

            _socialMediaRepository.Delete(socialMedia);
            await _socialMediaRepository.SaveChangesAsync();

            return true;
        }
    }
}