namespace PoundPupLegacy.Db.Writers;

public record UserGroup : AccessRole
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
