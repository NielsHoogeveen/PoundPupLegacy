using System.Data;

namespace PoundPupLegacy.Db.Readers;
public sealed class VocabularyIdReaderByOwnerAndNameFactory : IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName>
{
    public async Task<VocabularyIdReaderByOwnerAndName> CreateAsync(NpgsqlConnection connection)
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

}
public sealed class VocabularyIdReaderByOwnerAndName : SingleItemDatabaseReader<VocabularyIdReaderByOwnerAndName.VocabularyIdReaderByOwnerAndNameRequest, int>
{
    public record VocabularyIdReaderByOwnerAndNameRequest
    {
        public required int OwnerId { get; init; }
        public required string Name { get; init; }

    }

    internal VocabularyIdReaderByOwnerAndName(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(VocabularyIdReaderByOwnerAndNameRequest request)
    {
        _command.Parameters["owner_id"].Value = request.OwnerId;
        _command.Parameters["name"].Value = request.Name;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"vocabulary {request.Name} cannot be found for owner {request.OwnerId}");
    }
}
