namespace PoundPupLegacy.Model;

public interface Node
{
    public int Id { get; }
    public int UserId { get; }
    public DateTime Created { get; }
    public DateTime Changed { get; }
    public string Title { get; }
    public int Status { get; }
    public int NodeTypeId { get; }

}
