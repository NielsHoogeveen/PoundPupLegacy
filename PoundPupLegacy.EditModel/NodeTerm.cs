namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TagNodeType))]
public partial class TagNodeTypeJsonContext : JsonSerializerContext { }

public sealed record TagNodeType
{
    public required int[] NodeTypeIds { get; init; }

    public required string TagLabelName { get; init; }
}

public sealed record Tags
{
    public required TagNodeType TagNodeType { get; init; }


    private List<NodeTerm> entries = new();
    public List<NodeTerm> Entries { get => entries; init { if (value is not null) entries = value; } }
}

public abstract record NodeTerm
{
    private NodeTerm() { }
    public required int TermId { get; init; }

    public required string Name { get; init; }

    public required int NodeTypeId { get; init; }

    public abstract T Match<T>(
        Func<ExistingNodeTerm, T> existingNodeTerm,
        Func<NewNodeTerm, T> newNodeTerm
    );
    
    public sealed record ExistingNodeTerm: NodeTerm
    {
        public required int NodeId { get; set; }

        public bool HasBeenDeleted { get; set; } = false;

        public override T Match<T>(
            Func<ExistingNodeTerm, T> existingNodeTerm,
            Func<NewNodeTerm, T> newNodeTerm
        )
        {
            return existingNodeTerm(this);
        }
    }
    public sealed record NewNodeTerm: NodeTerm
    {
        public override T Match<T>(
            Func<ExistingNodeTerm, T> existingNodeTerm,
            Func<NewNodeTerm, T> newNodeTerm
        )
        {
            return newNodeTerm(this);
        }
    }
}
