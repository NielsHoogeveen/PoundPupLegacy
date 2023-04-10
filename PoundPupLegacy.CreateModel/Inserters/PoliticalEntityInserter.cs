namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PoliticalEntityInserterFactory : DatabaseInserterFactory<PoliticalEntity>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter FileIdFlag = new() { Name = "file_id_flag" };

    public override async Task<IDatabaseInserter<PoliticalEntity>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "political_entity",
            new DatabaseParameter[] {
                Id,
                FileIdFlag
            }
        );
        return new PoliticalEntityInserter(command);
    }
}
internal sealed class PoliticalEntityInserter : DatabaseInserter<PoliticalEntity>
{

    internal PoliticalEntityInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PoliticalEntity entity)
    {
        if (entity.Id is null)
            throw new NullReferenceException();

        Set(PoliticalEntityInserterFactory.Id, entity.Id.Value);
        Set(PoliticalEntityInserterFactory.FileIdFlag, entity.FileIdFlag);
        await _command.ExecuteNonQueryAsync();
    }
}
