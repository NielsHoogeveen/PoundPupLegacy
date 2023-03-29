namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class AccessRolePrivilegeInserter : DatabaseInserter<AccessRolePrivilege>, IDatabaseInserter<AccessRolePrivilege>
{

    private const string ACCESS_ROLE_ID = "access_role_id";
    private const string ACTION_ID = "action_id";
    public static async Task<DatabaseInserter<AccessRolePrivilege>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "access_role_privilege",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ACCESS_ROLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ACTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new AccessRolePrivilegeInserter(command);

    }

    internal AccessRolePrivilegeInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(AccessRolePrivilege createNodeAccessPrivilege)
    {
        WriteValue(createNodeAccessPrivilege.AccessRoleId, ACCESS_ROLE_ID);
        WriteNullableValue(createNodeAccessPrivilege.ActionId, ACTION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
