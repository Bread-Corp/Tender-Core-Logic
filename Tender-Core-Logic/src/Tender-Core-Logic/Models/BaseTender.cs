using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public abstract class BaseTender : ITender
    {
        [Key]
        [Required]
        public Guid TenderID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        public DateTime ClosingDate { get; set; }

        public List<Tag>? Tags { get; set; }

        public string? Description { get; set; }

        public string? SupportingDocs { get; set; }
    }
}
