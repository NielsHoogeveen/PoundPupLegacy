namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class AccessRolePrivilegeInserterFactory : DatabaseInserterFactory<AccessRolePrivilege>
{
    internal static NonNullableIntegerDatabaseParameter AccessRoleId = new() { Name = "access_role_id" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };

    public override async Task<IDatabaseInserter<AccessRolePrivilege>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "access_role_privilege",
            new DatabaseParameter[] {
                AccessRoleId,
                ActionId
            }
        );
        return new AccessRolePrivilegeInserter(command);

    }

}
internal sealed class AccessRolePrivilegeInserter : DatabaseInserter<AccessRolePrivilege>
{


    internal AccessRolePrivilegeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AccessRolePrivilege createNodeAccessPrivilege)
    {

        Set(AccessRolePrivilegeInserterFactory.AccessRoleId, createNodeAccessPrivilege.AccessRoleId);
        Set(AccessRolePrivilegeInserterFactory.ActionId, createNodeAccessPrivilege.ActionId);
        await _command.ExecuteNonQueryAsync();
    }
}
