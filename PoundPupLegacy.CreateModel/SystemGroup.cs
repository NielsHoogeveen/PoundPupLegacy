namespace PoundPupLegacy.CreateModel;

public sealed record SystemGroup : UserGroup, Owner
{
    public string Name => "System";

    public string Description => "Group for the maintenance of the system";

    public AdministratorRole AdministratorRole => new AdministratorRole {
        UserGroupId = 0,
        IdentificationForCreate = new Identification.Possible {
            Id = Constants.SYSTEM_ADMINISTRATOR,
        },
    };
    public required Vocabulary.ToCreate VocabularyTagging { get; init; }
    public required Identification.Possible IdentificationForCreate {
        get => new Identification.Possible { Id = Constants.OWNER_SYSTEM };
        init {
        }
    } 
    public Identification Identification => IdentificationForCreate;

}
