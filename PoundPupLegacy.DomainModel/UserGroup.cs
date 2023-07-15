namespace PoundPupLegacy.DomainModel;

public interface UserGroup : PossiblyIdentifiable
{
    public string Name { get; }
    public string Description { get; }
    public AdministratorRole AdministratorRole { get; }
}
