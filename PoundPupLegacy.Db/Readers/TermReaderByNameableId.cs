using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class TermReaderByNameableId : DatabaseReader, IDatabaseReader<TermReaderByNameableId>
{
    public static async Task<TermReaderByNameableId> CreateAsync(NpgsqlConnection connection)
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

    internal TermReaderByNameableId(NpgsqlCommand command) : base(command) { }

    public async Task<Term> ReadAsync(int ownerId, string vocabularyName, int nameableId)
    {
        _command.Parameters["owner_id"].Value = ownerId;
        _command.Parameters["vocabulary_name"].Value = vocabularyName;
        _command.Parameters["nameable_id"].Value = nameableId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var term = new Term {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                VocabularyId = ownerId,
                NameableId = nameableId
            };
            await reader.CloseAsync();
            return term;
        }
        await reader.CloseAsync();
        throw new Exception($"term {nameableId} cannot be found in vocabulary {vocabularyName} of owner {ownerId}");
    }
}
