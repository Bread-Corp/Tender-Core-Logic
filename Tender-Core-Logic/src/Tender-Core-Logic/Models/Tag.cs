using System.ComponentModel.DataAnnotations;

namespace Tender_Core_Logic.Models
{
    public class Tag
    {
        [Key]
        public Guid TagID { get; set; }

        [Required]
        public string TagName { get; set; }

        public List<BaseTender> Tenders { get; set; } = new();
    }
}
