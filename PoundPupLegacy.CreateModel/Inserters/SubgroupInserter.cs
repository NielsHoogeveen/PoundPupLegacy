namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SubgroupInserter : DatabaseInserter<Subgroup>, IDatabaseInserter<Subgroup>
{
    private const string ID = "id";
    private const string TENANT_ID = "tenant_id";
    public static async Task<DatabaseInserter<Subgroup>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "subgroup",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TENANT_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new SubgroupInserter(command);

    }

    internal SubgroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Subgroup subgroup)
    {
        if (subgroup.Id is null)
            throw new NullReferenceException();
        WriteValue(subgroup.Id, ID);
        WriteValue(subgroup.TenantId, TENANT_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
