namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UserGroupUserRoleUserInserterFactory : DatabaseInserterFactory<UserGroupUserRoleUser>
{
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableIntegerDatabaseParameter UserRoleId = new() { Name = "user_role_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    

    public override async Task<IDatabaseInserter<UserGroupUserRoleUser>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user_group_user_role_user",
            new DatabaseParameter[] {
                UserGroupId,
                UserRoleId,
                UserId
            }
        );
        return new UserGroupUserRoleUserInserter(command);
    }
}
internal sealed class UserGroupUserRoleUserInserter : DatabaseInserter<UserGroupUserRoleUser>
{
    internal UserGroupUserRoleUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UserGroupUserRoleUser userGroupUserRoleUser)
    {
        Set(UserGroupUserRoleUserInserterFactory.UserGroupId, userGroupUserRoleUser.UserGroupId);
        Set(UserGroupUserRoleUserInserterFactory.UserRoleId, userGroupUserRoleUser.UserRoleId);
        Set(UserGroupUserRoleUserInserterFactory.UserId, userGroupUserRoleUser.UserId);
        await _command.ExecuteNonQueryAsync();
    }
}
