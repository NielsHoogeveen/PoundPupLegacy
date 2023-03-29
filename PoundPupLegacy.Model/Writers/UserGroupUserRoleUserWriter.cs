namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class UserGroupUserRoleUserWriter : DatabaseWriter<UserGroupUserRoleUser>, IDatabaseWriter<UserGroupUserRoleUser>
{
    private const string USER_GROUP_ID = "user_group_id";
    private const string USER_ROLE_ID = "user_role_id";
    private const string USER_ID = "user_id";
    public static async Task<DatabaseWriter<UserGroupUserRoleUser>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "user_group_user_role_user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = USER_GROUP_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = USER_ROLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UserGroupUserRoleUserWriter(command);

    }

    internal UserGroupUserRoleUserWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(UserGroupUserRoleUser userGroupUserRoleUser)
    {
        WriteValue(userGroupUserRoleUser.UserGroupId, USER_GROUP_ID);
        WriteValue(userGroupUserRoleUser.UserRoleId, USER_ROLE_ID);
        WriteValue(userGroupUserRoleUser.UserId, USER_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
