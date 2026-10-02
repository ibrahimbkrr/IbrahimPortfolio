namespace IbrahimPortfolio.Entity.Entities
{
    public class Experience
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsCurrent { get; set; }
    }
}