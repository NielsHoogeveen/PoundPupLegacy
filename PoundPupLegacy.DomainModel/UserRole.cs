namespace PoundPupLegacy.DomainModel;


public interface UserRoleToCreate : UserRole, PrincipalToCreate
{

}
public interface UserRoleToUpdate : UserRole, PrincipalToUpdate
{

}
public interface UserRole : Principal
{
    string Name { get; }
    int? UserGroupId { get; }
}
