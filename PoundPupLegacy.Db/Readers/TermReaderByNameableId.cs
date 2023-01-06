using System.Data;

namespace PoundPupLegacy.Db.Readers;

public class TermReaderByNameableId : DatabaseReader<Term>, IDatabaseReader<TermReaderByNameableId>
{
    public static async Task<TermReaderByNameableId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id, name FROM term WHERE vocabulary_id = @vocabulary_id AND nameable_id = @nameable_id
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("vocabulary_id", NpgsqlDbType.Integer);
        command.Parameters.Add("nameable_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new TermReaderByNameableId(command);

    }

    internal TermReaderByNameableId(NpgsqlCommand command) : base(command) { }

    public async Task<Term> ReadAsync(int vocabularyId, int nameableId)
    {
        _command.Parameters["vocabulary_id"].Value = vocabularyId;
        _command.Parameters["nameable_id"].Value = nameableId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            var term = new Term
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                VocabularyId = vocabularyId,
                NameableId = nameableId
            };
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"term {nameableId} cannot be found in vocabulary {vocabularyId}");
    }
}
