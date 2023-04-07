namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantInserterFactory : DatabaseInserterFactory<Tenant>
{
    public override async Task<IDatabaseInserter<Tenant>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TenantInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TenantInserter.DOMAIN_NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TenantInserter.VOCABULARY_ID_TAGGING,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TenantInserter.ACCESS_ROLE_ID_NOT_LOGGED_IN,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TenantInserter(command);
    }
}
internal sealed class TenantInserter : DatabaseInserter<Tenant>
{
    internal const string ID = "id";
    internal const string DOMAIN_NAME = "domain_name";
    internal const string VOCABULARY_ID_TAGGING = "vocabulary_id_tagging";
    internal const string ACCESS_ROLE_ID_NOT_LOGGED_IN = "access_role_id_not_logged_in";


    internal TenantInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Tenant @tenant)
    {
        if (@tenant.Id is null)
            throw new NullReferenceException();
        SetParameter(@tenant.Id, ID);
        SetParameter(@tenant.DomainName, DOMAIN_NAME);
        SetNullableParameter(@tenant.VocabularyIdTagging, VOCABULARY_ID_TAGGING);
        SetParameter(@tenant.AccessRoleNotLoggedIn.Id, ACCESS_ROLE_ID_NOT_LOGGED_IN);
        await _command.ExecuteNonQueryAsync();
    }
}
