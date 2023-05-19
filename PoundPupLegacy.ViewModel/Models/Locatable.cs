namespace PoundPupLegacy.ViewModel.Models;

public abstract record LocatableBase: NameableBase, Locatable
{
    private Location[] _locations = Array.Empty<Location>();
    public required Location[] Locations {
        get => _locations;
        init {
            if (value is not null) {
                _locations = value;
            }
        }
    }
}


public interface Locatable: Nameable
{
    Location[] Locations { get; }
}
