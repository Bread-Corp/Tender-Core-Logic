using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class eTender : BaseTender
    {
        [Required]
        public string? TenderNumber { get; set; }

        public string? Department { get; set; }

        public string? URL { get; set; }
    }
}

