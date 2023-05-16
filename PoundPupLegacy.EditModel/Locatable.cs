namespace PoundPupLegacy.EditModel;

public abstract record LocatableBase: NameableBase, Locatable
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
    private List<Location> locations = new();

    public List<Location> Locations {
        get => locations;
        init {
            if (value is not null) {
                locations = value;
            }
        }
    }
}

public interface Locatable : Nameable
{
    List<Location> Locations { get; }

    List<CountryListItem> Countries { get; }
}
