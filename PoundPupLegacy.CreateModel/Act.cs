namespace PoundPupLegacy.CreateModel;

public abstract record Act : Nameable, Documentable
{
    private Act() { }
    public required ActDetails ActDetails { get; init; }
    public sealed record ToCreate : Act, NameableToCreate, DocumentableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : Act, NameableToUpdate, DocumentableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
public sealed record ActDetails
{
    public required DateTime? EnactmentDate { get; init; }
}
