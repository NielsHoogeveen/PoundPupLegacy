namespace PoundPupLegacy.CreateModel.Readers;
public sealed class TermReaderByNameFactory : IDatabaseReaderFactory<TermReaderByName>
{
    public async Task<TermReaderByName> CreateAsync(IDbConnection connection)
    {
        var sql = """
            SELECT id, nameable_id FROM term WHERE vocabulary_id = @vocabulary_id AND name = @name
            """;

        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("vocabulary_id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        await command.PrepareAsync();

        return new TermReaderByName(command);

    }

}
public sealed class TermReaderByName : SingleItemDatabaseReader<TermReaderByName.Request, Term>
{
    public record Request
    {
        public required int VocabularyId { get; init; }
        public required string Name { get; init; }

    }

    internal TermReaderByName(NpgsqlCommand command) : base(command) { }

    public override async Task<Term> ReadAsync(Request request)
    {
        if (request.Name is null) {
            throw new ArgumentNullException(nameof(request.Name));
        }
        _command.Parameters["vocabulary_id"].Value = request.VocabularyId;
        _command.Parameters["name"].Value = request.Name.Trim();

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var term = new Term {
                Id = reader.GetInt32("id"),
                Name = request.Name,
                VocabularyId = request.VocabularyId,
                NameableId = reader.GetInt32("nameable_id")
            };
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"term {request.Name} cannot be found in vocabulary {request.VocabularyId}");
    }
}
