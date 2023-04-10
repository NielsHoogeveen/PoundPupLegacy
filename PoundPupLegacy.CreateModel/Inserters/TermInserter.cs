namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermInserterFactory : DatabaseInserterFactory<Term>
{
    internal static NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    public override async Task<IDatabaseInserter<Term>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            "term",
            new DatabaseParameter[] {
                VocabularyId,
                Name,
                NameableId
            }
        );
        return new TermInserter(command);
    }
}
internal sealed class TermInserter : DatabaseInserter<Term>
{

    internal TermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Term term)
    {
        Set(TermInserterFactory.VocabularyId, term.VocabularyId);
        Set(TermInserterFactory.Name, term.Name.Trim());
        Set(TermInserterFactory.NameableId, term.NameableId);
        var retval = await _command.ExecuteScalarAsync();
        term.Id = retval switch {
            int i => i,
            long i => (int)i,
            _ => throw new Exception($"Id could not be set for term {term.Name} in vocabulary {term.VocabularyId}")
        };
    }
}
