namespace PoundPupLegacy.CreateModel.Writers;
public class UserRoleWriter : DatabaseWriter<UserRole>, IDatabaseWriter<UserRole>
{
    private const string ID = "id";
    private const string USER_GROUP_ID = "user_group_id";
    private const string NAME = "name";

    public static async Task<DatabaseWriter<UserRole>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "user_role",
            new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = USER_GROUP_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },

        });
        return new UserRoleWriter(command);
    }

    private UserRoleWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(UserRole userRole)
    {
        WriteValue(userRole.Id, ID);
        WriteValue(userRole.UserGroupId, USER_GROUP_ID);
        WriteValue(userRole.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
