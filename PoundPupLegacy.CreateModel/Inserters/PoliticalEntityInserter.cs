namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PoliticalEntityInserterFactory : DatabaseInserterFactory<PoliticalEntity>
{
    public override async Task<IDatabaseInserter<PoliticalEntity>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "political_entity",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PoliticalEntityInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PoliticalEntityInserter.FILE_ID_FLAG,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PoliticalEntityInserter(command);
    }
}
internal sealed class PoliticalEntityInserter : DatabaseInserter<PoliticalEntity>
{
    internal const string ID = "id";
    internal const string FILE_ID_FLAG = "file_id_flag";

    internal PoliticalEntityInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PoliticalEntity entity)
    {
        if (entity.Id is null)
            throw new NullReferenceException();

        SetParameter(entity.Id, ID);
        SetNullableParameter(entity.FileIdFlag, FILE_ID_FLAG);
        await _command.ExecuteNonQueryAsync();
    }
}
