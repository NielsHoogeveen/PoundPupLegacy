namespace PoundPupLegacy.Model;

public sealed record UserRole : AccessRole
{
    public required int? Id { get; set; }

    public required string Name { get; init; }

    public required int? UserGroupId { get; set; }

}
