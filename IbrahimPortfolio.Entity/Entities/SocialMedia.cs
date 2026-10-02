namespace IbrahimPortfolio.Entity.Entities
{
    public class SocialMedia
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}