namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserGroupUserRoleUserCreator : IEntityCreator<UserGroupUserRoleUser>
{
    public async Task CreateAsync(IAsyncEnumerable<UserGroupUserRoleUser> userGroupUserRoleUsers, IDbConnection connection)
    {

        await using var userGroupUserRoleUserWriter = await UserGroupUserRoleUserInserter.CreateAsync(connection);

        await foreach (var userGroupUserRoleUser in userGroupUserRoleUsers) {
            await userGroupUserRoleUserWriter.InsertAsync(userGroupUserRoleUser);
        }
    }
}
