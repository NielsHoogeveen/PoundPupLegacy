namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserGroupUserRoleUserCreator : EntityCreator<UserGroupUserRoleUser>
{
    private readonly IDatabaseInserterFactory<UserGroupUserRoleUser> _userGroupUserRoleUserInserterFactory;
    public UserGroupUserRoleUserCreator(IDatabaseInserterFactory<UserGroupUserRoleUser> userGroupUserRoleUserInserterFactory)
    {
        _userGroupUserRoleUserInserterFactory = userGroupUserRoleUserInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<UserGroupUserRoleUser> userGroupUserRoleUsers, IDbConnection connection)
    {
        await using var userGroupUserRoleUserWriter = await _userGroupUserRoleUserInserterFactory.CreateAsync(connection);

        await foreach (var userGroupUserRoleUser in userGroupUserRoleUsers) {
            await userGroupUserRoleUserWriter.InsertAsync(userGroupUserRoleUser);
        }
    }
}
