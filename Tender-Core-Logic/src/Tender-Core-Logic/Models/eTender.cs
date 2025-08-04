using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class eTender : BaseTender
    {
        [Required]
        public string TenderNumber { get; set; }

        public string ProcurementMethod { get; set; }

        public string ProcurementMethodDetails { get; set; }

        public string ProcuringEntity { get; set; }

        public string Currency { get; set; }

        public Decimal Value { get; set; }

        public string Category { get; set; }

        public string Tenderer { get; set; }
    }
}
