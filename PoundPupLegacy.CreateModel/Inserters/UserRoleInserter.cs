namespace PoundPupLegacy.CreateModel.Inserters;

public class UserRoleInserterFactory : DatabaseInserterFactory<UserRole>
{
    public override async Task<IDatabaseInserter<UserRole>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user_role",
            new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = UserRoleInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = UserRoleInserter.USER_GROUP_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = UserRoleInserter.NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },

        });
        return new UserRoleInserter(command);
    }
}
public class UserRoleInserter : DatabaseInserter<UserRole>
{
    internal const string ID = "id";
    internal const string USER_GROUP_ID = "user_group_id";
    internal const string NAME = "name";

    internal UserRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UserRole userRole)
    {
        WriteValue(userRole.Id, ID);
        WriteValue(userRole.UserGroupId, USER_GROUP_ID);
        WriteValue(userRole.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
