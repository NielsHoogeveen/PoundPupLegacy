namespace PoundPupLegacy.CreateModel;

public record AdministratorRole : UserRole
{
    public required int? Id { get; set; }
    public required int? UserGroupId { get; set; }
    public string Name => "Administrator";
}
