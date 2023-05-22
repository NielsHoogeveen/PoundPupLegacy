using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class SimpleTextNodeEditServiceBase<TEntity, TExisting, TNew, TCreate>(
    IDbConnection connection,
    ILogger logger,
    ITenantRefreshService tenantRefreshService,
    IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest> simpleTextNodeUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITextService textService
    ) : NodeEditServiceBase<TEntity, TExisting, TNew, TCreate>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
)
    where TEntity : class, SimpleTextNode
    where TExisting : TEntity, ExistingNode
    where TNew : TEntity, NewNode
    where TCreate : CreateModel.EventuallyIdentifiableSimpleTextNode
{

    protected abstract IEntityCreator<TCreate> EntityCreator { get; }

    protected abstract TCreate Map(TNew item);

    protected sealed override async Task<int> StoreNew(TNew simpleTextNode, NpgsqlConnection connection)
    {
        var item = Map(simpleTextNode);
        var items = new List<TCreate> { item };
        await EntityCreator.CreateAsync(items.ToAsyncEnumerable(), connection);
        foreach (var nodeTypeTopics in simpleTextNode.Tags) {
            foreach (var topic in nodeTypeTopics.Entries) {
                topic.NodeId = item.Id;
            }
        }
        return item.Id!.Value;
    }

    protected sealed override async Task StoreExisting(TExisting article, NpgsqlConnection connection)
    {
        await using var updater = await simpleTextNodeUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new SimpleTextNodeUpdaterRequest {
            Title = article.Title,
            Text = textService.FormatText(article.Text),
            Teaser = textService.FormatTeaser(article.Text),
            NodeId = article.NodeId
        });
    }
}
