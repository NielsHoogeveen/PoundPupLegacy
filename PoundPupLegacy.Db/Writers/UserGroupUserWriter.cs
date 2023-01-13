namespace PoundPupLegacy.Db.Writers;

internal sealed class UserGroupUserWriter : DatabaseWriter<UserGroupUserRoleUser>, IDatabaseWriter<UserGroupUserRoleUser>
{
    private const string USER_GROUP_ID = "user_group_id";
    private const string USER_ID = "user_id";
    public static async Task<DatabaseWriter<UserGroupUserRoleUser>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "user_group_user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = USER_GROUP_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UserGroupUserWriter(command);

    }

    internal UserGroupUserWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(UserGroupUserRoleUser article)
    {
        WriteValue(article.UserGroupId, USER_GROUP_ID);
        WriteValue(article.UserId, USER_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
