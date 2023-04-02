namespace PoundPupLegacy.CreateModel.Inserters;
public class AdministratorRoleInserter : DatabaseInserter<AdministratorRole>, IDatabaseInserter<AdministratorRole>
{
    private const string ID = "id";
    private const string USER_GROUP_ID = "user_group_id";
    public static async Task<DatabaseInserter<AdministratorRole>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "administrator_role",
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

        });
        return new AdministratorRoleInserter(command);
    }

    private AdministratorRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AdministratorRole administratorRole)
    {
        WriteValue(administratorRole.Id, ID);
        WriteValue(administratorRole.UserGroupId, USER_GROUP_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
