namespace Tender_Core_Logic.UserModels
{
    public class SuperUser : TenderUser
    {
        public string? Organisation { get; set; }

        public SuperUser()
        {
            this.IsSuperUser = true;
            this.Role = "SuperUser";
        }
    }
}
