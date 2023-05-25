namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableLocatable : Locatable, ImmediatelyIdentifiableNameable, ImmediatelyIdentifiableDocumentable
{
    List<EventuallyIdentifiableLocation> NewLocations { get; }

    List<int> LocationsToDelete { get; }

    List<ImmediatelyIdentifiableLocation> LocationsToUpdate { get; }
}

public interface EventuallyIdentifiableLocatable: Locatable, EventuallyIdentifiableNameable,EventuallyIdentifiableDocumentable 
{
    List<EventuallyIdentifiableLocation> NewLocations { get; }
}

public interface Locatable : Nameable, Documentable
{
}

public abstract record NewLocatableBase : NewNameableBase, EventuallyIdentifiableLocatable
{
    public required List<EventuallyIdentifiableLocation> NewLocations { get; init; }
}

public abstract record ExistingLocatableBase : ExistingNameableBase, ImmediatelyIdentifiableLocatable
{
    public required List<EventuallyIdentifiableLocation> NewLocations { get; init; }

    public required List<int> LocationsToDelete { get; init; }

    public required List<ImmediatelyIdentifiableLocation> LocationsToUpdate { get; init; }
}

