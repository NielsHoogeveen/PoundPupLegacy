namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserGroupUserRoleUserCreatorFactory(
    IDatabaseInserterFactory<UserGroupUserRoleUser> userGroupUserRoleUserInserterFactory
) : IEntityCreatorFactory<UserGroupUserRoleUser>
{
    public async Task<IEntityCreator<UserGroupUserRoleUser>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<UserGroupUserRoleUser>(new() 
        {
            await userGroupUserRoleUserInserterFactory.CreateAsync(connection)
        });
}
