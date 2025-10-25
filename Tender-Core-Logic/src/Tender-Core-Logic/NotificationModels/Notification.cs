using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tender_Core_Logic.Models;
using Tender_Core_Logic.UserModels;

namespace Tender_Core_Logic.NotificationModels
{
    public class Notification
    {
        [Key]
        [Required]
        public Guid NotificationID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public Guid FKTenderID { get; set; }

        [ForeignKey(nameof(FKTenderID))]
        public BaseTender FKTender { get; set; }

        [Required]
        public Guid FKUserID { get; set; }

        [ForeignKey(nameof(FKUserID))]
        public TenderUser FKUser { get; set; }
    }
}
