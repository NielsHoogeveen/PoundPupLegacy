namespace PoundPupLegacy.CreateModel;

public interface Subdivision : GeographicalEntity
{
    public string Name { get; }

    public int CountryId { get; }

    public int SubdivisionTypeId { get; }
}
