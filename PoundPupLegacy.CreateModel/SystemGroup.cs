namespace PoundPupLegacy.CreateModel;

public record SystemGroup : UserGroup, Owner
{
    public string Name => "System";

    public string Description => "Group for the maintenance of the system";

    public AdministratorRole AdministratorRole => new AdministratorRole {
        UserGroupId = 0,
        Id = 21
    };
    public required Vocabulary VocabularyTagging { get; init; }

    public int? Id { get; set; } = Constants.OWNER_SYSTEM;
}
