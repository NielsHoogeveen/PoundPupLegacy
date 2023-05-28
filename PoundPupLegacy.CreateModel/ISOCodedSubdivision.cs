namespace PoundPupLegacy.CreateModel;
public interface ISOCodedSubdivisionToUpdate : ISOCodedSubdivision, SubdivisionToUpdate, PoliticalEntityToUpdate
{
}
public interface ISOCodedSubdivisionToCreate: ISOCodedSubdivision, SubdivisionToCreate, PoliticalEntityToCreate
{
}
public interface ISOCodedSubdivision : Subdivision, PoliticalEntity
{
    ISOCodedSubdivisionDetails ISOCodedSubdivisionDetails { get; }
}
public sealed record ISOCodedSubdivisionDetails
{
    public required string ISO3166_2_Code { get; init; }

    public required int? FileIdFlag { get; init; }
}
