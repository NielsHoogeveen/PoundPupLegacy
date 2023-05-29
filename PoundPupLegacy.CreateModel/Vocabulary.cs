namespace PoundPupLegacy.CreateModel;

public abstract record Vocabulary : Node
{
    private Vocabulary() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public required VocabularyDetails VocabularyDetails { get; init; }
    public abstract T Match<T>(Func<VocabularyToCreate, T> create, Func<VocabularyToUpdate, T> update);
    public abstract void Match(Action<VocabularyToCreate> create, Action<VocabularyToUpdate> update);

    public sealed record VocabularyToCreate : Vocabulary, NodeToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(Func<VocabularyToCreate, T> create, Func<VocabularyToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<VocabularyToCreate> create, Action<VocabularyToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record VocabularyToUpdate : Vocabulary, NodeToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override T Match<T>(Func<VocabularyToCreate, T> create, Func<VocabularyToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<VocabularyToCreate> create, Action<VocabularyToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record VocabularyDetails
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int OwnerId { get; init; }
}
