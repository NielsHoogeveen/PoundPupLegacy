namespace PoundPupLegacy.CreateModel;

public sealed record SystemGroup : UserGroup, Owner
{
    public string Name => "System";

    public string Description => "Group for the maintenance of the system";

    public AdministratorRole AdministratorRole => new AdministratorRole {
        UserGroupId = 0,
        IdentificationForCreate = new Identification.IdentificationForCreate {
            Id = Constants.SYSTEM_ADMINISTRATOR,
        },
    };
    public required Vocabulary.VocabularyToCreate VocabularyTagging { get; init; }
    public required Identification.IdentificationForCreate IdentificationForCreate {
        get => new Identification.IdentificationForCreate { Id = Constants.OWNER_SYSTEM };
        init {
        }
    } 
    public Identification Identification => IdentificationForCreate;

}
