namespace PoundPupLegacy.Model;

public record SystemGroup : UserGroup
{
    public string Name => "System";

    public string Description => "Group for the maintenance of the system";

    public AdministratorRole AdministratorRole => new AdministratorRole {
        UserGroupId = 0,
        Id = 21
    };

    public int? Id { get; set; } = 0;
}
