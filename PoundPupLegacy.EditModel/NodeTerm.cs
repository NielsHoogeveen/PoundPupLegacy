namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TagNodeType))]
public partial class TagNodeTypeJsonContext : JsonSerializerContext { }

public sealed record TagNodeType
{
    public required int[] NodeTypeIds { get; init; }

    public required string TagLabelName { get; init; }
}

public abstract record Tags
{
    public required TagNodeType TagNodeType { get; init; }

    public abstract IEnumerable<NodeTerm> Entries { get; }

    private List<NodeTerm.ForCreate> entries = new();
    public List<NodeTerm.ForCreate> EntriesToCreate { get => entries; init { if (value is not null) entries = value; } }

    public sealed record ToCreate: Tags
   {

        public override IEnumerable<NodeTerm> Entries => EntriesToCreate;
    }
    public sealed record ToUpdate: Tags
    {
        private List<NodeTerm.ForUpdate> entriesToUpdate = new();
        public List<NodeTerm.ForUpdate> EntriesToUpdate { get => entriesToUpdate; init { if (value is not null) entriesToUpdate = value; } }
        public override IEnumerable<NodeTerm> Entries => GetEntries();

        public IEnumerable<NodeTerm> GetEntries()
        {
            foreach(var elem in EntriesToUpdate) {
                yield return elem;
            }
            foreach (var elem in EntriesToCreate) {
                yield return elem;
            }
        }
    }

}

public abstract record NodeTerm
{
    private NodeTerm() { }
    public required int TermId { get; init; }

    public required string Name { get; init; }

    public required int NodeTypeId { get; init; }

    public abstract T Match<T>(
        Func<ForUpdate, T> existingNodeTerm,
        Func<ForCreate, T> newNodeTerm
    );
    
    public sealed record ForUpdate: NodeTerm
    {
        public required int NodeId { get; set; }

        public bool HasBeenDeleted { get; set; } = false;

        public override T Match<T>(
            Func<ForUpdate, T> existingNodeTerm,
            Func<ForCreate, T> newNodeTerm
        )
        {
            return existingNodeTerm(this);
        }
    }
    public sealed record ForCreate: NodeTerm
    {
        public override T Match<T>(
            Func<ForUpdate, T> existingNodeTerm,
            Func<ForCreate, T> newNodeTerm
        )
        {
            return newNodeTerm(this);
        }
    }
}
