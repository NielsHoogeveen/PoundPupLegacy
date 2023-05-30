namespace PoundPupLegacy.CreateModel;

public interface NameableToUpdate : Nameable, SearchableToUpdate
{
    NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; }
}
public interface NameableToCreate: Nameable, SearchableToCreate
{
    NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; }
}
public interface Nameable : Searchable
{
    NameableDetails NameableDetails { get; }
}
public abstract record NameableDetails
{
    private NameableDetails() { }
    public required string Description { get; init; }
    public required int? FileIdTileImage { get; init; }
    public abstract T Match<T>(Func<NameableDetailsForCreate, T> create, Func<NameableDetailsForUpdate, T> update);
    public abstract void Match(Action<NameableDetailsForCreate> create, Action<NameableDetailsForUpdate> update);
    public sealed record NameableDetailsForCreate: NameableDetails
    {
        public required List<NewTermForNewNameable> Terms { get; init; }
        public override T Match<T>(Func<NameableDetailsForCreate, T> create, Func<NameableDetailsForUpdate, T> update) 
        { 
            return create(this); 
        }
        public override void Match(Action<NameableDetailsForCreate> create, Action<NameableDetailsForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record NameableDetailsForUpdate : NameableDetails
    {
        public required List<NewTermForExistingNameable> TermsToAdd { get; init; }
        public override T Match<T>(Func<NameableDetailsForCreate, T> create, Func<NameableDetailsForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<NameableDetailsForCreate> create, Action<NameableDetailsForUpdate> update)
        {
            update(this);
        }
    }
}

