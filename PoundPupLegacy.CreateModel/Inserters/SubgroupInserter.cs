namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SubgroupInserterFactory : DatabaseInserterFactory<Subgroup>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };

    public override async Task<IDatabaseInserter<Subgroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "subgroup",
            new DatabaseParameter[] {
                Id,
                TenantId
            }
        );
        return new SubgroupInserter(command);
    }
}
internal sealed class SubgroupInserter : DatabaseInserter<Subgroup>
{
    internal SubgroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Subgroup subgroup)
    {
        if (subgroup.Id is null)
            throw new NullReferenceException();
        Set(SubgroupInserterFactory.Id, subgroup.Id.Value);
        Set(SubgroupInserterFactory.TenantId, subgroup.TenantId);
        await _command.ExecuteNonQueryAsync();
    }
}
