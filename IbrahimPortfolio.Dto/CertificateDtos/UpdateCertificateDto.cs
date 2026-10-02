using System.ComponentModel.DataAnnotations;
using IbrahimPortfolio.Dto.Validation;
namespace IbrahimPortfolio.Dto.CertificateDtos
{
    public class UpdateCertificateDto : IValidatableObject
    {

        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200)]
        public string Issuer { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }


        public DateTime? ExpirationDate { get; set; }

        [StringLength(200)]
        public string? CredentialId { get; set; }

        [StringLength(2048)]
        [Url]
        [SafeUrl]
        public string? CredentialUrl { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IssueDate < new DateTime(1900, 1, 1))
                yield return new ValidationResult("Geçerli bir tarih girin.", new[] { nameof(IssueDate) });

            if (ExpirationDate.HasValue && ExpirationDate.Value < IssueDate)
                yield return new ValidationResult("Bitiş tarihi başlangıç tarihinden önce olamaz.", new[] { nameof(ExpirationDate) });
        }
    }
}
