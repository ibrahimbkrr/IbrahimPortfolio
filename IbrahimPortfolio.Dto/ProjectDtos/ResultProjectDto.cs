namespace IbrahimPortfolio.Dto.ProjectDtos
{
    public class ResultProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? GithubUrl { get; set; }

        public string? LiveUrl { get; set; }

        public string Technologies { get; set; } = string.Empty;
    }
}