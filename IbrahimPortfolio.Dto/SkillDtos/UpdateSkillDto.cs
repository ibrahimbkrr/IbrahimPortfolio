using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
namespace IbrahimPortfolio.Dto.SkillDtos
{
    public class UpdateSkillDto
    {

        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Level { get; set; }
    }
}