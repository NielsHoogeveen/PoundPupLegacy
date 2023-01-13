using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class VocabularyIdReaderByOwnerAndName : DatabaseReader<Term>, IDatabaseReader<VocabularyIdReaderByOwnerAndName>
{
    public static async Task<VocabularyIdReaderByOwnerAndName> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id FROM vocabulary WHERE owner_id = @owner_id AND name = @name
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("owner_id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        await command.PrepareAsync();

        return new VocabularyIdReaderByOwnerAndName(command);

    }

    internal VocabularyIdReaderByOwnerAndName(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(int ownerId, string name)
    {
        _command.Parameters["owner_id"].Value = ownerId;
        _command.Parameters["name"].Value = name;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"vocabulary {name} cannot be found for owner {ownerId}");
    }
}
