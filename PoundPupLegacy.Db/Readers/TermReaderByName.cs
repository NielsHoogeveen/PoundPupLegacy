using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class TermReaderByName : DatabaseUpdater<Term>, IDatabaseReader<TermReaderByName>
{
    public static async Task<TermReaderByName> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id, nameable_id FROM term WHERE vocabulary_id = @vocabulary_id AND name = @name
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("vocabulary_id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        await command.PrepareAsync();

        return new TermReaderByName(command);

    }

    internal TermReaderByName(NpgsqlCommand command) : base(command) { }

    public async Task<Term> ReadAsync(int vocabularyId, string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        _command.Parameters["vocabulary_id"].Value = vocabularyId;
        _command.Parameters["name"].Value = name.Trim();

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            var term = new Term
            {
                Id = reader.GetInt32("id"),
                Name = name,
                VocabularyId = vocabularyId,
                NameableId = reader.GetInt32("nameable_id")
            };
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"term {name} cannot be found in vocabulary {vocabularyId}");
    }
}
