using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
namespace IbrahimPortfolio.Dto.SocialMediaDtos
{
    public class UpdateSocialMediaDto
    {

        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(2048)]
        [Url]
        [SafeUrl]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Icon { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}