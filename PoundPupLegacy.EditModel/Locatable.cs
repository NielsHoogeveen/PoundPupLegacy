namespace PoundPupLegacy.EditModel;

public abstract record NewLocatableBase : NewNameableBase, NewLocatable
{

    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }

    private List<NewLocation> locations = new();

    public List<NewLocation> LocationsToAdd {
        get => locations;
        init {
            if (value is not null) {
                locations = value;
            }
        }
    }
    public IEnumerable<Location> Locations => LocationsToAdd;
    public void Add(NewLocation location)
    {
        LocationsToAdd.Add(location);
    }

}

public abstract record ExistingLocatableBase : ExistingNameableBase, ExistingLocatable
{
    private List<NewLocation> locationsToAdd = new();

    public List<NewLocation> LocationsToAdd {
        get => locationsToAdd;
        init {
            if (value is not null) {
                locationsToAdd = value;
            }
        }
    }

    private List<ExistingLocation> locationsToUpdate = new();

    public List<ExistingLocation> LocationsToUpdate {
        get => locationsToUpdate;
        init {
            if (value is not null) {
                locationsToUpdate = value;
            }
        }
    }
    public IEnumerable<Location> Locations => GetLocations();
    private IEnumerable<Location> GetLocations()
    {
        foreach (var location in locationsToAdd) {
            yield return location;
        }
        foreach (var location in locationsToUpdate) {
            yield return location;
        }
    }

    public void Add(NewLocation location)
    {
        LocationsToAdd.Add(location);
    }

    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }
}
public interface NewLocatable : Locatable, NewNode
{
    List<NewLocation> LocationsToAdd { get; }

}
public interface ExistingLocatable : Locatable, ExistingNode
{
    List<NewLocation> LocationsToAdd { get; }
    List<ExistingLocation> LocationsToUpdate { get; }

}

public interface Locatable : Nameable
{
    IEnumerable<Location> Locations { get; }
    List<CountryListItem> Countries { get; }

    void Add(NewLocation location);
}
