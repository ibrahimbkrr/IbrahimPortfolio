namespace IbrahimPortfolio.Dto.CertificateDtos
{
    public class ResultCertificateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
    }
}