using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class SarsTender : BaseTender
    {
        [Required]
        public string? TenderNumber { get; set; }

        public string? BriefingSession { get; set; }
    }
}
