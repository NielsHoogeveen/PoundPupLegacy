namespace PoundPupLegacy.CreateModel;

public sealed record AccessRole : UserRole
{
    public required int? Id { get; set; }

    public required string Name { get; init; }

    public required int? UserGroupId { get; set; }

}
