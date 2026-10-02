using AutoMapper;
using IbrahimPortfolio.Dto.AboutDtos;
using IbrahimPortfolio.Dto.CertificateDtos;
using IbrahimPortfolio.Dto.EducationDtos;
using IbrahimPortfolio.Dto.ExperienceDtos;
using IbrahimPortfolio.Dto.MessageDtos;
using IbrahimPortfolio.Dto.ProjectDtos;
using IbrahimPortfolio.Dto.SkillDtos;
using IbrahimPortfolio.Dto.SocialMediaDtos;
using IbrahimPortfolio.Entity.Entities;
namespace IbrahimPortfolio.Business.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Project, ResultProjectDto>();

            CreateMap<CreateProjectDto, Project>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UpdateProjectDto, Project>();

            CreateMap<About, ResultAboutDto>();
            CreateMap<CreateAboutDto, About>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<UpdateAboutDto, About>();

            CreateMap<Experience, ResultExperienceDto>();
            CreateMap<CreateExperienceDto, Experience>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<UpdateExperienceDto, Experience>();

            CreateMap<Education, ResultEducationDto>();
            CreateMap<CreateEducationDto, Education>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<UpdateEducationDto, Education>();

            CreateMap<Skill, ResultSkillDto>();
            CreateMap<CreateSkillDto, Skill>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<UpdateSkillDto, Skill>();

            CreateMap<Certificate, ResultCertificateDto>();
            CreateMap<CreateCertificateDto, Certificate>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<UpdateCertificateDto, Certificate>();

            CreateMap<SocialMedia, ResultSocialMediaDto>();
            CreateMap<CreateSocialMediaDto, SocialMedia>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<UpdateSocialMediaDto, SocialMedia>();

            CreateMap<Message, ResultMessageDto>();
            CreateMap<CreateMessageDto, Message>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.CreatedAt, opt => opt.Ignore()).ForMember(x => x.IsRead, opt => opt.Ignore());
        }
    }
}

