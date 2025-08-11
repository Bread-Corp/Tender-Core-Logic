namespace Tender_Core_Logic.Models
{
    public interface ITender
    {
        Guid TenderID { get; }
        string Title { get; }
        string Status { get; }
        DateTime PublishedDate { get; }
        DateTime ClosingDate { get; }
        DateTime DateAppended { get; }
        string Source { get; }
        List<Tag> Tags { get; }
        string? Description { get; }
        string? SupportingDocs { get; }
    }
}
