namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class VocabularyInserterFactory : DatabaseInserterFactory<Vocabulary>
{
    public override async Task<IDatabaseInserter<Vocabulary>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "vocabulary",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = VocabularyInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = VocabularyInserter.OWNER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = VocabularyInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = VocabularyInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new VocabularyInserter(command);
    }
}
internal sealed class VocabularyInserter : DatabaseInserter<Vocabulary>
{
    internal const string ID = "id";
    internal const string OWNER_ID = "owner_id";
    internal const string NAME = "name";
    internal const string DESCRIPTION = "description";

    internal VocabularyInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Vocabulary vocabulary)
    {
        if (vocabulary.Id is null)
            throw new NullReferenceException();
        SetParameter(vocabulary.Id, ID);
        SetParameter(vocabulary.OwnerId, OWNER_ID);
        SetParameter(vocabulary.Name, NAME);
        SetParameter(vocabulary.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
