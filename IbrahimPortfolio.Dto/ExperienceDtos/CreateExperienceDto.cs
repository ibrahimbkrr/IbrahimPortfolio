using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbrahimPortfolio.Dto.ExperienceDtos
{
    public class CreateExperienceDto : IValidatableObject
    {

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate < new DateTime(1900, 1, 1))
                yield return new ValidationResult("Geçerli bir tarih girin.", new[] { nameof(StartDate) });

            if (EndDate.HasValue && EndDate.Value < StartDate)
                yield return new ValidationResult("Bitiş tarihi başlangıç tarihinden önce olamaz.", new[] { nameof(EndDate) });
        }
    }
}

