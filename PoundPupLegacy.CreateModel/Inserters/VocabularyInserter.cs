namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class VocabularyInserterFactory : DatabaseInserterFactory<Vocabulary>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };

    public override async Task<IDatabaseInserter<Vocabulary>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "vocabulary",
            new DatabaseParameter[] {
                Id,
                OwnerId,
                Name,
                Description
            }
        );
        return new VocabularyInserter(command);
    }
}
internal sealed class VocabularyInserter : DatabaseInserter<Vocabulary>
{
    internal VocabularyInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Vocabulary vocabulary)
    {
        if (vocabulary.Id is null)
            throw new NullReferenceException();
        Set(VocabularyInserterFactory.Id, vocabulary.Id.Value);
        Set(VocabularyInserterFactory.OwnerId, vocabulary.OwnerId);
        Set(VocabularyInserterFactory.Name, vocabulary.Name);
        Set(VocabularyInserterFactory.Description, vocabulary.Description);
        await _command.ExecuteNonQueryAsync();
    }
}
