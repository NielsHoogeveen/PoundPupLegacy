namespace PoundPupLegacy.CreateModel;

public abstract record Page : SimpleTextNode
{
    private Page() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : Page, SimpleTextNodeToCreate
    {
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record ToUpdate : Page, SimpleTextNodeToUpdate
    {
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}
