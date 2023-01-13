namespace PoundPupLegacy.Model;

public sealed record AdoptionExportYear
{
    public required int AdoptionExportRelationId { get; init; }
    public required int Year { get; init; }
    public required int NumberOfChildren { get; init; }
}
