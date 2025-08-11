using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tender_Core_Logic.Models
{
    [NotMapped]
    public class SupportingDoc
    {
        public string Name { get; set; }
        public string URL { get; set; }
    }
}
