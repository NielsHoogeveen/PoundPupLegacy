namespace PoundPupLegacy.CreateModel;
public interface PollToUpdate : Poll, NodeToUpdate
{
}

public interface PollToCreate : Poll, NodeToCreate
{
}

public interface Poll : Node
{
    PollDetails PollDetails { get; }
}
public sealed record PollDetails
{
    public required int PollStatusId { get; init; }
    public required DateTime DateTimeClosure { get; init; }
}
