using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class SanralTender : BaseTender
    {
        [Required]
        public string? TenderNumber { get; set; }

        public string? Category { get; set; }

        public string? Location { get; set; }

        public string? Email { get; set; }

        public string? FullTextNotice { get; set; }
    }
}
