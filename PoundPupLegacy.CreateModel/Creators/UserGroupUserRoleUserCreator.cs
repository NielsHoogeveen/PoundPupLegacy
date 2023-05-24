namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserGroupUserRoleUserCreatorFactory(
    IDatabaseInserterFactory<UserGroupUserRoleUser> userGroupUserRoleUserInserterFactory
) : IInsertingEntityCreatorFactory<UserGroupUserRoleUser>
{
    public async Task<InsertingEntityCreator<UserGroupUserRoleUser>> CreateAsync(IDbConnection connection) =>
        new(new() 
        {
            await userGroupUserRoleUserInserterFactory.CreateAsync(connection)
        });
}
