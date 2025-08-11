using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tender_Core_Logic.Models
{
    public abstract class BaseTender : ITender
    {
        [Key]
        [Required]
        public Guid TenderID { get; set; } //Initialised on create.

        [Required]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; } //Set on create.

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        public DateTime ClosingDate { get; set; }

        [Required]
        public DateTime DateAppended { get; set; }

        [Required]
        [JsonProperty("Source")]
        public string Source { get; set; } //Appended manually from lambda function.

        public List<Tag> Tags { get; set; } = new(); //Appended from comprehend.

        public string? Description { get; set; } //AI summary possibly.

        public string? SupportingDocs { get; set; }
    }
}
