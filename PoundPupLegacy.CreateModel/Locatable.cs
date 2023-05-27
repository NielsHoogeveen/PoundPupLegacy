namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableLocatable : Locatable, ImmediatelyIdentifiableNameable, ImmediatelyIdentifiableDocumentable
{
    List<EventuallyIdentifiableLocation> LocationsToAdd { get; }

    List<int> LocationsToDelete { get; }

    List<ImmediatelyIdentifiableLocation> LocationsToUpdate { get; }
}

public interface EventuallyIdentifiableLocatable: Locatable, EventuallyIdentifiableNameable,EventuallyIdentifiableDocumentable 
{
    List<EventuallyIdentifiableLocation> Locations { get; }
}

public interface Locatable : Nameable, Documentable
{
}

public abstract record NewLocatableBase : NewNameableBase, EventuallyIdentifiableLocatable
{
    public required List<EventuallyIdentifiableLocation> Locations { get; init; }
}

public abstract record ExistingLocatableBase : ExistingNameableBase, ImmediatelyIdentifiableLocatable
{
    public required List<EventuallyIdentifiableLocation> LocationsToAdd { get; init; }

    public required List<int> LocationsToDelete { get; init; }

    public required List<ImmediatelyIdentifiableLocation> LocationsToUpdate { get; init; }
}

