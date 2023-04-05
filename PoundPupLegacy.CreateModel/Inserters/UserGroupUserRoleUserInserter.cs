namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UserGroupUserRoleUserInserterFactory : DatabaseInserterFactory<UserGroupUserRoleUser>
{
    public override async Task<IDatabaseInserter<UserGroupUserRoleUser>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user_group_user_role_user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = UserGroupUserRoleUserInserter.USER_GROUP_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UserGroupUserRoleUserInserter.USER_ROLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UserGroupUserRoleUserInserter.USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UserGroupUserRoleUserInserter(command);
    }
}
internal sealed class UserGroupUserRoleUserInserter : DatabaseInserter<UserGroupUserRoleUser>
{
    internal const string USER_GROUP_ID = "user_group_id";
    internal const string USER_ROLE_ID = "user_role_id";
    internal const string USER_ID = "user_id";

    internal UserGroupUserRoleUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UserGroupUserRoleUser userGroupUserRoleUser)
    {
        WriteValue(userGroupUserRoleUser.UserGroupId, USER_GROUP_ID);
        WriteValue(userGroupUserRoleUser.UserRoleId, USER_ROLE_ID);
        WriteValue(userGroupUserRoleUser.UserId, USER_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
