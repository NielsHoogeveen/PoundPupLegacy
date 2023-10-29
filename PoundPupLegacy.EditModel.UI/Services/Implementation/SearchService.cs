using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class SearchService<TListItem, TRequest>(
    NpgsqlDataSource dataSource,
    ILogger logger,
    IEnumerableDatabaseReaderFactory<TRequest, TListItem> readerFactory
) : DatabaseService(dataSource, logger), ISearchService<TListItem>
    where TListItem : EditListItem
    where TRequest : IRequest
{

    protected abstract TRequest GetRequest(int tenantId, string searchString);
    public async Task<List<TListItem>> GetItems(int tenantId, string searchString)
    {
        
        List<TListItem> items = new();
        return await WithSequencedConnection(async (connection) => {
            await using var reader = await readerFactory.CreateAsync(connection);
            await foreach (var elem in reader.ReadAsync(GetRequest(tenantId, searchString))) {
                items.Add(elem);
            }
            return items;

        });
    }
}
