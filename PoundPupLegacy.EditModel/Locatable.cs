namespace PoundPupLegacy.EditModel;

public interface Locatable : Node
{
    List<Location> Locations { get; }

    List<CountryListItem> Countries { get; }
}
