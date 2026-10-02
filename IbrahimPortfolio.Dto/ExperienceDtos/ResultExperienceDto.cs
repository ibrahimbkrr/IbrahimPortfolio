using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbrahimPortfolio.Dto.ExperienceDtos
{
    public class ResultExperienceDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }
    }
}
