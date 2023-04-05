namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class AdministratorRoleInserterFactory : DatabaseInserterFactory<AdministratorRole>
{
    public override async Task<IDatabaseInserter<AdministratorRole>> CreateAsync(IDbConnection connection)
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
                Name = AdministratorRoleInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = AdministratorRoleInserter.USER_GROUP_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },

        });
        return new AdministratorRoleInserter(command);
    }

}
internal class AdministratorRoleInserter : DatabaseInserter<AdministratorRole>
{
    internal const string ID = "id";
    internal const string USER_GROUP_ID = "user_group_id";

    internal AdministratorRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AdministratorRole administratorRole)
    {
        WriteValue(administratorRole.Id, ID);
        WriteValue(administratorRole.UserGroupId, USER_GROUP_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
