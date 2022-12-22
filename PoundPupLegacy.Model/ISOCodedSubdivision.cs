namespace PoundPupLegacy.Model;

public interface ISOCodedSubdivision : Subdivision
{
    public string ISO3166_2_Code { get; }
}
