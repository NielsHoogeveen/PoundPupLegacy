namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class UserGroupUserRoleUserCreatorFactory(
    IDatabaseInserterFactory<UserRoleUser> userGroupUserRoleUserInserterFactory
) : IEntityCreatorFactory<UserRoleUser>
{
    public async Task<IEntityCreator<UserRoleUser>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<UserRoleUser>(new()
        {
            await userGroupUserRoleUserInserterFactory.CreateAsync(connection)
        });
}
