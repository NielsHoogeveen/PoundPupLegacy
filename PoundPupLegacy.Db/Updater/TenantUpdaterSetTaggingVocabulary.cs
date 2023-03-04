using System.Data;

namespace PoundPupLegacy.Db.Updaters;

public sealed class TenantUpdaterSetTaggingVocabulary : DatabaseUpdater<Term>, IDatabaseUpdater<TenantUpdaterSetTaggingVocabulary>
{
    public static async Task<TenantUpdaterSetTaggingVocabulary> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            UPDATE tenant SET vocabulary_id_tagging = @vocabulary_id WHERE id = @tenant_id
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("vocabulary_id", NpgsqlDbType.Integer);

        await command.PrepareAsync();

        return new TenantUpdaterSetTaggingVocabulary(command);

    }

    internal TenantUpdaterSetTaggingVocabulary(NpgsqlCommand command) : base(command) { }

    public async Task Update(int tenantId, int vocabularyId)
    {
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["vocabulary_id"].Value = vocabularyId;

        var count = await _command.ExecuteNonQueryAsync();
        if (count != 1) {
            throw new Exception($"Unexpected {count} rows were updated setting vocaburaly {vocabularyId} for tenant {tenantId}");
        }
    }
}
