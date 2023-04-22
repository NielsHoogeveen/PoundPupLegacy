using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class SimpleTextNodeEditServiceBase<T, TCreate> : NodeEditServiceBase<T, TCreate>
    where T : SimpleTextNode
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

    protected abstract TCreate Map(T item);

    protected sealed override async Task StoreNew(T simpleTextNode, NpgsqlConnection connection)
    {

        var item = Map(simpleTextNode);
        var items = new List<TCreate> { item };
        await EntityCreator.CreateAsync(items.ToAsyncEnumerable(), connection);
        simpleTextNode.UrlId = item.Id;
        foreach (var topic in simpleTextNode.Tags) {
            topic.NodeId = simpleTextNode.UrlId;
        }
        await _tagSaveService.SaveAsync(simpleTextNode.Tags, connection);
    }

    protected sealed override async Task StoreExisting(T article, NpgsqlConnection connection)
    {
        await using var updater = await _simpleTextNodeUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new SimpleTextNodeUpdaterRequest {
            Title = article.Title,
            Text = _textService.FormatText(article.Text),
            Teaser = _textService.FormatTeaser(article.Text),
            NodeId = article.NodeId!.Value
        });
        await _tagSaveService.SaveAsync(article.Tags, connection);
    }

}
