﻿namespace PoundPupLegacy.Db.Writers;

internal sealed class TenantWriter : DatabaseWriter<Tenant>, IDatabaseWriter<Tenant>
{
    private const string ID = "id";
    private const string DOMAIN_NAME = "domain_name";
    private const string VOCABULARY_ID_TAGGING = "vocabulary_id_tagging";
    private const string ACCESS_ROLE_ID_NOT_LOGGED_IN = "access_role_id_not_logged_in";

    public static async Task<DatabaseWriter<Tenant>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "tenant",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DOMAIN_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = VOCABULARY_ID_TAGGING,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ACCESS_ROLE_ID_NOT_LOGGED_IN,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
               
            }
        );
        return new TenantWriter(command);

    }

    internal TenantWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Tenant @tenant)
    {
        if (@tenant.Id is null)
            throw new NullReferenceException();
        WriteValue(@tenant.Id, ID);
        WriteValue(@tenant.DomainName, DOMAIN_NAME);
        WriteNullableValue(@tenant.VocabularyIdTagging, VOCABULARY_ID_TAGGING);
        WriteValue(@tenant.AccessRoleNotLoggedIn.Id, ACCESS_ROLE_ID_NOT_LOGGED_IN);
        await _command.ExecuteNonQueryAsync();
    }
}
