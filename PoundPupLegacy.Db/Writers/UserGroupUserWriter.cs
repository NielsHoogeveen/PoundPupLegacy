namespace PoundPupLegacy.Db.Writers;

internal class UserGroupUserWriter : DatabaseWriter<UserGroupUser>, IDatabaseWriter<UserGroupUser>
{
    private const string USER_GROUP_ID = "user_group_id";
    private const string USER_ID = "user_id";
    public static DatabaseWriter<UserGroupUser> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(UserGroupUser article)
    {
        try
        {
            WriteValue(article.UserGroupId, USER_GROUP_ID);
            WriteValue(article.UserId, USER_ID);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
