namespace PoundPupLegacy.CreateModel;

public sealed record AccessRole : UserRole
{
    public required string Name { get; init; }

    public required int? UserGroupId { get; set; }

    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

    public Identification Identification => IdentificationForCreate;
}
