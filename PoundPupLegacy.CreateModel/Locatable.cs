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
    public sealed record LocatableDetailsForCreate : LocatableDetails
    {
        public required List<EventuallyIdentifiableLocation> Locations { get; init; }
    }
    public abstract record LocatableDetailsForUpdate : LocatableDetails
    {
        public required List<EventuallyIdentifiableLocation> LocationsToAdd { get; init; }

        public required List<int> LocationsToDelete { get; init; }

        public required List<ImmediatelyIdentifiableLocation> LocationsToUpdate { get; init; }
    }
}