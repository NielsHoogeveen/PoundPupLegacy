namespace PoundPupLegacy.CreateModel;

public interface SubdivisionToUpdate : Subdivision, GeographicalEntityToUpdate
{
}
public interface SubdivisionToCreate: Subdivision, GeographicalEntityToCreate
{
}
public interface Subdivision : GeographicalEntity
{
    SubdivisionDetails SubdivisionDetails { get; }
}
public sealed record SubdivisionDetails
{
    public required string Name { get; init; }
    public required int CountryId { get; init; }
    public required int SubdivisionTypeId { get; init; }
}