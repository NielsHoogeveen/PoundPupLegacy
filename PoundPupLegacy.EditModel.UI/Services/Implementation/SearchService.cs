using Npgsql;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class SearchService<TListItem, TRequest> : ISearchService<TListItem>
    where TListItem : EditListItem
    where TRequest: IRequest
{
    private readonly NpgsqlConnection _connection;

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private readonly IEnumerableDatabaseReaderFactory<TRequest, TListItem> _readerFactory;


    protected abstract TRequest GetRequest(int tenantId, string searchString);
    public SearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<TRequest, TListItem> readerFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _readerFactory = readerFactory;
    }
    public async Task<List<TListItem>> GetItems(int tenantId, string searchString)
    {
        await semaphore.WaitAsync();
        List<TListItem> items = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await _readerFactory.CreateAsync(_connection);
            await foreach (var elem in reader.ReadAsync(GetRequest(tenantId, searchString))) {
                items.Add(elem);
            }
            return items;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
            semaphore.Release();
        }
    }
}
