using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class SearchService<TListItem, TRequest>(
    IDbConnection connection,
    ILogger logger,
    IEnumerableDatabaseReaderFactory<TRequest, TListItem> readerFactory
) : DatabaseService(connection, logger), ISearchService<TListItem>
    where TListItem : EditListItem
    where TRequest : IRequest
{

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    protected abstract TRequest GetRequest(int tenantId, string searchString);
    public async Task<List<TListItem>> GetItems(int tenantId, string searchString)
    {
        await semaphore.WaitAsync();
        List<TListItem> items = new();
        try {
            await connection.OpenAsync();
            await using var reader = await readerFactory.CreateAsync(connection);
            await foreach (var elem in reader.ReadAsync(GetRequest(tenantId, searchString))) {
                items.Add(elem);
            }
            return items;
        }
        finally {
            if (connection.State == ConnectionState.Open) {
                await connection.CloseAsync();
            }
            semaphore.Release();
        }
    }
}
