namespace PoundPupLegacy.CreateModel;

public interface LocatableToUpdate : Locatable, NameableToUpdate, DocumentableToUpdate
{
    LocatableDetails.ForUpdate LocatableDetails { get; }
}

public interface LocatableToCreate: Locatable, NameableToCreate,DocumentableToCreate 
{
    LocatableDetails.ForCreate LocatableDetails { get; }
}

public interface Locatable : Nameable, Documentable
{
}

public abstract record LocatableDetails
{
    private LocatableDetails() { }
    public sealed record ForCreate : LocatableDetails
    {
        public required List<Location.ToCreate> Locations { get; init; }
    }
    public sealed record ForUpdate : LocatableDetails
    {
        public required List<Location.ToCreate> LocationsToAdd { get; init; }

        public required List<int> LocationsToDelete { get; init; }

        public required List<Location.ToUpdate> LocationsToUpdate { get; init; }
    }
}