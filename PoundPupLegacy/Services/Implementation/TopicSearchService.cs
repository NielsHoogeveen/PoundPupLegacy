using Npgsql;
using System.Data;
using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services.Implementation;

public class TopicSearchService : ITopicSearchService
{
    private readonly ISiteDataService _siteDataService;
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<TopicSearchService> _logger;

    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

    public TopicSearchService(NpgsqlConnection connection, ILogger<TopicSearchService> logger, ISiteDataService siteDataService)
    {
        _siteDataService = siteDataService;
        _connection = connection;
        _logger = logger;
    }
    public async Task<List<Tag>> GetTerms(int nodeId, string str)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        await _connection.OpenAsync();
        List<Tag> tags = new();
        try
        {
            var sql = """
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
            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("search_string", NpgsqlTypes.NpgsqlDbType.Varchar);
            await readCommand.PrepareAsync();
            readCommand.Parameters["tenant_id"].Value = _siteDataService.GetTenantId();
            readCommand.Parameters["search_string"].Value = $"%{str}%";
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tags.Add(new Tag 
                { 
                    Name = reader.GetString(1), 
                    NodeId = nodeId, 
                    TermId = reader.GetInt32(0), 
                    HasBeenDeleted = false, 
                    IsStored = false,
                });
            }
            return tags;
        }
        finally
        {
            await _connection.CloseAsync();
            semaphore.Release();
        }
    }
}
