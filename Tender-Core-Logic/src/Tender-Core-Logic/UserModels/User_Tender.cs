using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tender_Core_Logic.Models;

namespace Tender_Core_Logic.UserModels
{
    public class User_Tender
    {
        [Key]
        public Guid WatchlistID { get; set; } = Guid.NewGuid();

        //indicates whether the tender is on the users watchlist
        [Required]
        public bool IsWatched { get; set; } = false;

        //per-user tender status -- does not affect the tender globally
        [Required]
        public string UserTenderStatus { get; set; } = "Watching";

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
