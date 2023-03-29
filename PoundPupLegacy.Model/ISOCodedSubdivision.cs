namespace PoundPupLegacy.CreateModel;

public interface ISOCodedSubdivision : Subdivision, PoliticalEntity
{
    public string ISO3166_2_Code { get; }
}
