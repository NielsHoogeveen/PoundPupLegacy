namespace PoundPupLegacy.Db.Writers;

internal class SubgroupWriter : DatabaseWriter<Subgroup>, IDatabaseWriter<Subgroup>
{
    private const string ID = "id";
    private const string TENANT_ID = "tenant_id";
    public static async Task<DatabaseWriter<Subgroup>> CreateAsync(NpgsqlConnection connection)
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
        return new SubgroupWriter(command);

    }

    internal SubgroupWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Subgroup subgroup)
    {
        if (subgroup.Id is null)
            throw new NullReferenceException();
        WriteValue(subgroup.Id, ID);
        WriteValue(subgroup.TenantId, TENANT_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
