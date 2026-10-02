namespace IbrahimPortfolio.Dto.EducationDtos
{
    public class ResultEducationDto
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