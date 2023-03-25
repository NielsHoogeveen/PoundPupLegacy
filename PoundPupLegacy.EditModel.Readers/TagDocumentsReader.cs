using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class TagDocumentsReader : DatabaseReader, IDatabaseReader<TagDocumentsReader>
{
    private TagDocumentsReader(NpgsqlCommand command) : base(command)
    {
    }
    public async IAsyncEnumerable<Tag> ReadAsync(int? nodeId, int tenantId, string str)
    {
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["search_string"].Value = $"%{str}%";
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            yield return new Tag {
                Name = reader.GetString(1),
                NodeId = nodeId,
                TermId = reader.GetInt32(0),
                HasBeenDeleted = false,
                IsStored = false,
            };
        }
    }
    public static async Task<TagDocumentsReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("search_string", NpgsqlTypes.NpgsqlDbType.Varchar);
        await command.PrepareAsync();
        return new TagDocumentsReader(command);
    }
    const string SQL = """
        select
        distinct
        *
        from(
            select
            t.id,
            t.name
            from term t
            join tenant tt on tt.id = @tenant_id
            where t.vocabulary_id = tt.vocabulary_id_tagging and t.name = @search_string
            union
            select
            t.id,
            t.name
            from term t
            join tenant tt on tt.id = @tenant_id
            where t.vocabulary_id = tt.vocabulary_id_tagging and t.name ilike @search_string
            LIMIT 50
        ) x
        """;

}
