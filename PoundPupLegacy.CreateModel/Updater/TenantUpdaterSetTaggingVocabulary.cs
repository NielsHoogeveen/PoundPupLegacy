namespace PoundPupLegacy.CreateModel.Updaters;

internal sealed class TenantUpdaterSetTaggingVocabularyFactory : IDatabaseUpdaterFactory<TenantUpdaterSetTaggingVocabulary>
{
    public async Task<TenantUpdaterSetTaggingVocabulary> CreateAsync(IDbConnection connection)
    {
        var sql = """
            UPDATE tenant SET vocabulary_id_tagging = @vocabulary_id WHERE id = @tenant_id
            """;

        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("vocabulary_id", NpgsqlDbType.Integer);

        await command.PrepareAsync();

        return new TenantUpdaterSetTaggingVocabulary(command);

    }

}
public sealed class TenantUpdaterSetTaggingVocabulary : DatabaseUpdater<TenantUpdaterSetTaggingVocabulary.Request>
{
    public record Request
    {
        public required int TenantId { get; init; }
        public required int VocabularyId { get; init; }
    }

    internal TenantUpdaterSetTaggingVocabulary(NpgsqlCommand command) : base(command) { }

    public override async Task UpdateAsync(Request request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["vocabulary_id"].Value = request.VocabularyId;

        var count = await _command.ExecuteNonQueryAsync();
        if (count != 1) {
            throw new Exception($"Unexpected {count} rows were updated setting vocaburaly {request.VocabularyId} for tenant {request.TenantId}");
        }
    }
}
