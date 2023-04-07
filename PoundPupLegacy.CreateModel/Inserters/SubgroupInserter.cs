namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SubgroupInserterFactory : DatabaseInserterFactory<Subgroup>
{
    public override async Task<IDatabaseInserter<Subgroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "subgroup",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = SubgroupInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SubgroupInserter.TENANT_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new SubgroupInserter(command);
    }
}
internal sealed class SubgroupInserter : DatabaseInserter<Subgroup>
{
    internal const string ID = "id";
    internal const string TENANT_ID = "tenant_id";

    internal SubgroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Subgroup subgroup)
    {
        if (subgroup.Id is null)
            throw new NullReferenceException();
        SetParameter(subgroup.Id, ID);
        SetParameter(subgroup.TenantId, TENANT_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
