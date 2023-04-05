namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class AccessRolePrivilegeInserterFactory : DatabaseInserterFactory<AccessRolePrivilege>
{
    public override async Task<IDatabaseInserter<AccessRolePrivilege>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "access_role_privilege",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = AccessRolePrivilegeInserter.ACCESS_ROLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = AccessRolePrivilegeInserter.ACTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new AccessRolePrivilegeInserter(command);

    }

}
internal sealed class AccessRolePrivilegeInserter : DatabaseInserter<AccessRolePrivilege>
{

    internal const string ACCESS_ROLE_ID = "access_role_id";
    internal const string ACTION_ID = "action_id";

    internal AccessRolePrivilegeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AccessRolePrivilege createNodeAccessPrivilege)
    {
        WriteValue(createNodeAccessPrivilege.AccessRoleId, ACCESS_ROLE_ID);
        WriteNullableValue(createNodeAccessPrivilege.ActionId, ACTION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
