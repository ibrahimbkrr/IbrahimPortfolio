using AutoMapper;
using IbrahimPortfolio.Business.Abstract;
using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.Dto.CertificateDtos;
using IbrahimPortfolio.Entity.Entities;

namespace IbrahimPortfolio.Business.Concrete
{
    public class CertificateManager : ICertificateService
    {
        private readonly IGenericRepository<Certificate> _certificateRepository;
        private readonly IMapper _mapper;

        public CertificateManager(
            IGenericRepository<Certificate> certificateRepository,
            IMapper mapper)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
        }

        public async Task<List<ResultCertificateDto>> GetAllAsync()
        {
            var certificates = await _certificateRepository.GetAllAsync();
            return _mapper.Map<List<ResultCertificateDto>>(certificates);
        }

        public async Task<ResultCertificateDto?> GetByIdAsync(int id)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);

            if (certificate == null)
                return null;

            return _mapper.Map<ResultCertificateDto>(certificate);
        }

        public async Task CreateAsync(CreateCertificateDto createCertificateDto)
        {
            var certificate = _mapper.Map<Certificate>(createCertificateDto);

            await _certificateRepository.CreateAsync(certificate);
            await _certificateRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateCertificateDto updateCertificateDto)
        {
            var certificate =
                await _certificateRepository.GetByIdAsync(updateCertificateDto.Id);

            if (certificate == null)
                return false;

            _mapper.Map(updateCertificateDto, certificate);

            _certificateRepository.Update(certificate);
            await _certificateRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);

            if (certificate == null)
                return false;

            _certificateRepository.Delete(certificate);
            await _certificateRepository.SaveChangesAsync();

            return true;
        }
    }
}