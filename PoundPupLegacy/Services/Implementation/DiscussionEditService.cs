using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Updaters;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

public class DiscussionEditService : SimpleTextNodeEditServiceBase<Discussion, CreateModel.Discussion>, IEditService<Discussion>
{
    private readonly IDatabaseReaderFactory<DiscussionCreateDocumentReader> _createDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<DiscussionUpdateDocumentReader> _updateDocumentReaderFactory;

    public DiscussionEditService(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        IDatabaseReaderFactory<DiscussionCreateDocumentReader> createDocumentReaderFactory,
        IDatabaseReaderFactory<DiscussionUpdateDocumentReader> updateDocumentReaderFactory,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdater> simpleTextNodeUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService,
        ILogger<DiscussionEditService> logger
    ): base(connection, siteDataService, nodeCacheService, simpleTextNodeUpdaterFactory, tagSaveService, tenantNodesSaveService, filesSaveService, textService, logger)
    {
        _createDocumentReaderFactory = createDocumentReaderFactory;
        _updateDocumentReaderFactory = updateDocumentReaderFactory;
    }

    protected override IEntityCreator<CreateModel.Discussion> EntityCreator => new DiscussionCreator();

    public async Task<Discussion> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeCreateDocumentRequest {
                NodeTypeId = Constants.ARTICLE,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
    public async Task<Discussion> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _updateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }

    }

    protected override CreateModel.Discussion Map(Discussion item)
    {
        var now = DateTime.Now;
        return new CreateModel.Discussion {
            Id = null,
            Title = item.Title,
            Text = _textService.FormatText(item.Text),
            Teaser = _textService.FormatTeaser(item.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DISCUSSION,
            OwnerId = item.OwnerId,
            PublisherId = item.PublisherId,
            TenantNodes = item.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
    }
}