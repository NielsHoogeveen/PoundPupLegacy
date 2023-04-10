namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantInserterFactory : DatabaseInserterFactory<Tenant>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter DomainName = new() { Name = "domain_name" };
    internal static NullableIntegerDatabaseParameter VocabularyIdTagging = new() { Name = "vocabulary_id_tagging" };
    internal static NonNullableIntegerDatabaseParameter AccessRoleIdNotLoggedIn = new() { Name = "access_role_id_not_logged_in" };

    public override async Task<IDatabaseInserter<Tenant>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant",
            new DatabaseParameter[] {
                Id,
                DomainName,
                VocabularyIdTagging,
                AccessRoleIdNotLoggedIn
            }
        );
        return new TenantInserter(command);
    }
}
internal sealed class TenantInserter : DatabaseInserter<Tenant>
{
    internal TenantInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Tenant @tenant)
    {
        if (@tenant.Id is null)
            throw new NullReferenceException();
        if (@tenant.AccessRoleNotLoggedIn.Id is null)
            throw new NullReferenceException();

        Set(TenantInserterFactory.Id, @tenant.Id.Value);
        Set(TenantInserterFactory.DomainName, @tenant.DomainName);
        Set(TenantInserterFactory.VocabularyIdTagging, @tenant.VocabularyIdTagging);
        Set(TenantInserterFactory.AccessRoleIdNotLoggedIn, @tenant.AccessRoleNotLoggedIn.Id.Value);
        await _command.ExecuteNonQueryAsync();
    }
}
