using Tender_Core_Logic.Models;

namespace Tender_Core_Logic.UserModels
{
	public class StandardUser : TenderUser
	{
		public string? Address { get; set; }

		public List<Tag> Tags { get; set; } = new List<Tag>();
	}
}