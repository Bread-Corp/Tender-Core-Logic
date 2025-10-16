using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class TransnetTender : BaseTender
    {
        [Required]
        public string? TenderNumber { get; set; }

        public string? Category { get; set; }

        public string? Region { get; set; }

        public string? Email { get; set; }

        public string? FullNoticeText { get; set; }
    }
}
