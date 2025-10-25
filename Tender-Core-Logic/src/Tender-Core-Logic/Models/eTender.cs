using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class eTender : BaseTender
    {
        [Required]
        public string? TenderNumber { get; set; }

        public string? Audience { get; set; }

        public string? OfficeLocation { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Province { get; set; }
    }
}

