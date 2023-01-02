namespace PoundPupLegacy.Model;

public record UserGroup : AccessRole
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
}
