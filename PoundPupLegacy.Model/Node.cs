namespace PoundPupLegacy.Model;

public interface Node : Identifiable
{
    public int AccessRoleId { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime ChangedDateTime { get; }
    public string Title { get; }
    public int NodeStatusId { get; }
    public int NodeTypeId { get; }

}
