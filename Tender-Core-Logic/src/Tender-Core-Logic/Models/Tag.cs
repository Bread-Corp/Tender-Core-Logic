using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tender_Core_Logic.UserModels;

namespace Tender_Core_Logic.Models
{
    public class Tag
    {
        [Key]
        public Guid TagID { get; set; }

        [Required]
        public string TagName { get; set; }

        [JsonIgnore]
        public List<BaseTender> Tenders { get; set; } = new();

        [JsonIgnore]
        public List<StandardUser> StandardUsers { get; set; } = new();
    }
}
