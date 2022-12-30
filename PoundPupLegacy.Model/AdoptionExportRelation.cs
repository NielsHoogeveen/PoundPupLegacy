namespace PoundPupLegacy.Model;

public record AdoptionExportRelation
{
    public required int CountryIdTo { get; init; }
    public required int? CountryIdFrom { get; init; }
    public required string? CountryNameFrom { get; init; }
}
