﻿namespace PoundPupLegacy.Db.Writers;
public class AdministratorRoleWriter : DatabaseWriter<AdministratorRole>, IDatabaseWriter<AdministratorRole>
{
    private const string ID = "id";
    private const string USER_GROUP_ID = "user_group_id";
    public static async Task<DatabaseWriter<AdministratorRole>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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
        return new AdministratorRoleWriter(command);
    }

    private AdministratorRoleWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(AdministratorRole administratorRole)
    {
        WriteValue(administratorRole.Id, ID);
        WriteValue(administratorRole.UserGroupId, USER_GROUP_ID);
        await _command.ExecuteNonQueryAsync();
    }
}