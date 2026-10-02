namespace IbrahimPortfolio.Dto.AboutDtos
{
    public class ResultAboutDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? CvUrl { get; set; }
    }
}