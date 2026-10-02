using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
namespace IbrahimPortfolio.Dto.AboutDtos
{
    public class CreateAboutDto
    {

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(2048)]
        [SafeUrl(AllowLocal = true)]
        public string? ImageUrl { get; set; }

        [StringLength(40)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(254)]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(2048)]
        [SafeUrl(AllowLocal = true)]
        public string? CvUrl { get; set; }
    }
}