namespace PoundPupLegacy.CreateModel.Readers;

public sealed class TermReaderByNameableIdFactory : IDatabaseReaderFactory<TermReaderByNameableId>
{
    public async Task<TermReaderByNameableId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT 
                t.id, 
                t.name 
            FROM term t
            JOIN vocabulary v on v.id = t.vocabulary_id
            WHERE v.owner_id = @owner_id AND v.name = @vocabulary_name AND nameable_id = @nameable_id
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("owner_id", NpgsqlDbType.Integer);
        command.Parameters.Add("vocabulary_name", NpgsqlDbType.Varchar);
        command.Parameters.Add("nameable_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new TermReaderByNameableId(command);

    }

}
public sealed class TermReaderByNameableId : SingleItemDatabaseReader<TermReaderByNameableId.Request, Term>
{

    public record Request
    {
        public required int OwnerId { get; init; }
        public required string VocabularyName { get; init; }
        public required int NameableId { get; init; }
    }
    internal TermReaderByNameableId(NpgsqlCommand command) : base(command) { }

    public override async Task<Term> ReadAsync(Request request)
    {
        _command.Parameters["owner_id"].Value = request.OwnerId;
        _command.Parameters["vocabulary_name"].Value = request.VocabularyName;
        _command.Parameters["nameable_id"].Value = request.NameableId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var term = new Term {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                VocabularyId = request.OwnerId,
                NameableId = request.NameableId
            };
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"term {request.NameableId} cannot be found in vocabulary {request.VocabularyName} of owner {request.OwnerId}");
    }
}
