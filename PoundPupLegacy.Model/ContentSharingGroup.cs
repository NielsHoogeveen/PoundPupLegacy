namespace PoundPupLegacy.Model;

public record ContentSharingGroup: Owner
{
    public required int? Id { get; set; }
    public required string Name { get; init; }

    public required string Description { get; init; }
}
