namespace PoundPupLegacy.CreateModel;

public sealed record AdministratorRole : UserRoleToCreate
{
    public required Identification.Possible Identification { get; init; }
    public required int? UserGroupId { get; set; }
    public string Name => "Administrator";
}
