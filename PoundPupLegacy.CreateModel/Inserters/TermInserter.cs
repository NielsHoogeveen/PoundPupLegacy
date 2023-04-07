namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermInserterFactory : DatabaseInserterFactory<Term>
{
    public override async Task<IDatabaseInserter<Term>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TermInserter.NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = TermInserter.VOCABULARY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TermInserter.NAMEABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TermInserter(command);
    }
}
internal sealed class TermInserter : DatabaseInserter<Term>
{
    internal const string VOCABULARY_ID = "vocabulary_id";
    internal const string NAME = "name";
    internal const string NAMEABLE_ID = "nameable_id";

    internal TermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Term term)
    {
        SetParameter(term.VocabularyId, VOCABULARY_ID);
        SetParameter(term.Name.Trim(), NAME);
        SetParameter(term.NameableId, NAMEABLE_ID);
        var retval = await _command.ExecuteScalarAsync();
        term.Id = retval switch {
            int i => i,
            long i => (int)i,
            _ => throw new Exception($"Id could not be set for term {term.Name} in vocabulary {term.VocabularyId}")
        };
    }
}
