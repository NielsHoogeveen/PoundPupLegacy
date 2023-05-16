namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class SimpleTextNodeEditServiceBase<TEntity, TExisting, TNew, TCreate> : NodeEditServiceBase<TEntity, TExisting, TNew, TCreate>
    where TEntity: class, SimpleTextNode
    where TExisting : TEntity, ExistingNode
    where TNew : TEntity, NewNode
    where TCreate : CreateModel.SimpleTextNode
{
    private readonly IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest> _simpleTextNodeUpdaterFactory;
    protected readonly ITextService _textService;


    protected SimpleTextNodeEditServiceBase(
        IDbConnection connection,
        ITenantRefreshService tenantRefreshService,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest> simpleTextNodeUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService
        ) : base(
            connection,
            tagSaveService,
            tenantNodesSaveService,
            filesSaveService,
            tenantRefreshService
    )
    {
        _simpleTextNodeUpdaterFactory = simpleTextNodeUpdaterFactory;
        _textService = textService;
    }

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
        await using var updater = await _simpleTextNodeUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new SimpleTextNodeUpdaterRequest {
            Title = article.Title,
            Text = _textService.FormatText(article.Text),
            Teaser = _textService.FormatTeaser(article.Text),
            NodeId = article.NodeId
        });
    }

}
