namespace PoundPupLegacy.CreateModel;

public sealed record AdministratorRole : UserRole
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;

    public required int? UserGroupId { get; set; }
    public string Name => "Administrator";
}
