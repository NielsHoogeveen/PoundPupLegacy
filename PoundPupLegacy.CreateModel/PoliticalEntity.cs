namespace PoundPupLegacy.DomainModel;
public interface PoliticalEntityToUpdate : GeographicalEntityToUpdate, PoliticalEntity
{
}
public interface PoliticalEntityToCreate : PoliticalEntity, GeographicalEntityToCreate
{
}
public interface PoliticalEntity : GeographicalEntity
{
    PoliticalEntityDetails PoliticalEntityDetails { get; }
}
public sealed record PoliticalEntityDetails
{
    public required int? FileIdFlag { get; init; }
}
