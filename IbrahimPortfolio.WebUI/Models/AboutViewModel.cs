using IbrahimPortfolio.Dto.AboutDtos;
using IbrahimPortfolio.Dto.SocialMediaDtos;

namespace IbrahimPortfolio.WebUI.Models
{
    public class AboutViewModel
    {
        public ResultAboutDto? About { get; set; }

        public List<ResultSocialMediaDto> SocialMedias { get; set; }
            = new List<ResultSocialMediaDto>();
    }
}