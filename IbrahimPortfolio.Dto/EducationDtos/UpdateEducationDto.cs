using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
namespace IbrahimPortfolio.Dto.EducationDtos
{
    public class UpdateEducationDto : IValidatableObject
    {

        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string SchoolName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Degree { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate < new DateTime(1900, 1, 1))
                yield return new ValidationResult("Geçerli bir tarih girin.", new[] { nameof(StartDate) });

            if (EndDate.HasValue && EndDate.Value < StartDate)
                yield return new ValidationResult("Bitiş tarihi başlangıç tarihinden önce olamaz.", new[] { nameof(EndDate) });
        }
    }
}
