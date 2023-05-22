namespace PoundPupLegacy.CreateModel;

public interface UserGroup : EventuallyIdentifiable
{
    public string Name { get; }
    public string Description { get; }

    public AdministratorRole AdministratorRole { get; }

}
