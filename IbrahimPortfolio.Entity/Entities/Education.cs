namespace IbrahimPortfolio.Entity.Entities
{
    public class Education
    {
        public int Id { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Degree { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}