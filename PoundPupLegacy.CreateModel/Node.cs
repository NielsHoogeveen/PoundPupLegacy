namespace PoundPupLegacy.CreateModel;

public interface Node : Identifiable
{
    public int PublisherId { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime ChangedDateTime { get; }
    public string Title { get; }
    public int NodeTypeId { get; }
    public int OwnerId { get; }
    public List<TenantNode> TenantNodes { get; }
    public int AuthoringStatusId { get; }

}
