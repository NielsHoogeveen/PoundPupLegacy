namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserGroupUserRoleUserCreator(IDatabaseInserterFactory<UserGroupUserRoleUser> userGroupUserRoleUserInserterFactory) : EntityCreator<UserGroupUserRoleUser>
{
    public override async Task CreateAsync(IAsyncEnumerable<UserGroupUserRoleUser> userGroupUserRoleUsers, IDbConnection connection)
    {
        await using var userGroupUserRoleUserWriter = await userGroupUserRoleUserInserterFactory.CreateAsync(connection);

        await foreach (var userGroupUserRoleUser in userGroupUserRoleUsers) {
            await userGroupUserRoleUserWriter.InsertAsync(userGroupUserRoleUser);
        }
    }
}
