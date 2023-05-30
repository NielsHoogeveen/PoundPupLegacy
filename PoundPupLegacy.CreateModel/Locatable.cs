namespace PoundPupLegacy.CreateModel;

public interface LocatableToUpdate : Locatable, NameableToUpdate, DocumentableToUpdate
{
    LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; }
}

public interface LocatableToCreate: Locatable, NameableToCreate,DocumentableToCreate 
{
    LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; }
}

public interface Locatable : Nameable, Documentable
{
}

public abstract record LocatableDetails
{
    private LocatableDetails() { }
    public abstract T Match<T>(Func<LocatableDetailsForCreate, T> create, Func<LocatableDetailsForUpdate, T> update);
    public abstract void Match(Action<LocatableDetailsForCreate> create, Action<LocatableDetailsForUpdate> update);

    public sealed record LocatableDetailsForCreate : LocatableDetails
    {
        public required List<EventuallyIdentifiableLocation> Locations { get; init; }
        public override T Match<T>(Func<LocatableDetailsForCreate, T> create, Func<LocatableDetailsForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<LocatableDetailsForCreate> create, Action<LocatableDetailsForUpdate> update)
        {
            create(this);
        }
    }
    public abstract record LocatableDetailsForUpdate : LocatableDetails
    {
        public required List<EventuallyIdentifiableLocation> LocationsToAdd { get; init; }

        public required List<int> LocationsToDelete { get; init; }

        public required List<ImmediatelyIdentifiableLocation> LocationsToUpdate { get; init; }
        public override T Match<T>(Func<LocatableDetailsForCreate, T> create, Func<LocatableDetailsForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<LocatableDetailsForCreate> create, Action<LocatableDetailsForUpdate> update)
        {
            update(this);
        }
    }
}