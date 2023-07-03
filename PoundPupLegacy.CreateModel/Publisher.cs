namespace PoundPupLegacy.CreateModel;

public interface PublisherToCreate: Publisher, PrincipalToCreate
{

}
public interface PublisherToUpdate : Publisher, PrincipalToUpdate
{

}
public interface Publisher : Principal
{
    public string Name { get; }
}
