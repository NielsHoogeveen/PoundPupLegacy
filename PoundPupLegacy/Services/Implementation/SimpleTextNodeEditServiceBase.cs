using PoundPupLegacy.Updaters;
using System.Data;
using Npgsql;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services.Implementation;

public abstract class SimpleTextNodeEditServiceBase<T,TCreate>
    where T: SimpleTextNode
    where TCreate : CreateModel.SimpleTextNode
{
    protected readonly NpgsqlConnection _connection;
    private readonly IDatabaseUpdaterFactory<SimpleTextNodeUpdater> _simpleTextNodeUpdaterFactory;
    private readonly ISaveService<IEnumerable<Tag>> _tagSaveService;
    protected readonly ITextService _textService;
    protected readonly ILogger _logger;

    protected SimpleTextNodeEditServiceBase(
        IDbConnection connection,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdater> simpleTextNodeUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ITextService textService,
        ILogger logger
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _simpleTextNodeUpdaterFactory = simpleTextNodeUpdaterFactory;
        _tagSaveService = tagSaveService;
        _textService = textService;
        _logger = logger;
    }

    protected abstract IEntityCreator<TCreate> EntityCreator { get; }

    protected abstract TCreate Map(T item);

    public async Task Save(T simpleTextNode)
    {
        try {
            await _connection.OpenAsync();
            await using var tx = await _connection.BeginTransactionAsync();
            try {
                if (simpleTextNode.NodeId is null) {
                    await StoreNew(simpleTextNode, _connection);
                }
                else {
                    await StoreExisting(simpleTextNode, _connection);
                }
                await tx.CommitAsync();
            }
            catch (Exception ex) {
                await tx.RollbackAsync();
                _logger.LogError(ex, $"Error saving {typeof(T)}");
                throw;
            }
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }

    }
    private async Task StoreNew(T simpleTextNode, NpgsqlConnection connection)
    {
        
        var item = Map(simpleTextNode);
        var items = new List<TCreate> { item };
        await EntityCreator.CreateAsync(items.ToAsyncEnumerable(), connection);
        simpleTextNode.UrlId = item.Id;
        foreach (var topic in simpleTextNode.Tags) {
            topic.NodeId = simpleTextNode.UrlId;
        }
        await _tagSaveService.Save(simpleTextNode.Tags, connection);
    }

    private async Task StoreExisting(T article, NpgsqlConnection connection)
    {
        await using var updater = await _simpleTextNodeUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new SimpleTextNodeUpdater.Request {
            Title = article.Title,
            Text = _textService.FormatText(article.Text),
            Teaser = _textService.FormatTeaser(article.Text),
            NodeId = article.NodeId!.Value
        });
        await _tagSaveService.Save(article.Tags, connection);
    }

}
