namespace PoundPupLegacy.Model;

public interface Subdivision : GeographicalEntity
{
    public string Name { get; }

    public int CountryId { get; }
}
