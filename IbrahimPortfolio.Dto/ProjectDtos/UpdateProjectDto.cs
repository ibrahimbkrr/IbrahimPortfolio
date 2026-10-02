using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
namespace IbrahimPortfolio.Dto.ProjectDtos
{
    public class UpdateProjectDto
    {

        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(2048)]
        [SafeUrl(AllowLocal = true)]
        public string? ImageUrl { get; set; }

        [StringLength(2048)]
        [Url]
        [SafeUrl]
        public string? GithubUrl { get; set; }

        [StringLength(2048)]
        [Url]
        [SafeUrl]
        public string? LiveUrl { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(1000)]
        public string Technologies { get; set; } = string.Empty;
    }
}