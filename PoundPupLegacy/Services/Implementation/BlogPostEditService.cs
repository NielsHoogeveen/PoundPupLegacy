using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Updaters;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

public class BlogPostEditService : SimpleTextNodeEditServiceBase<BlogPost, CreateModel.BlogPost>, IEditService<BlogPost>
{
    private readonly IDatabaseReaderFactory<BlogPostCreateDocumentReader> _createDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<BlogPostUpdateDocumentReader> _updateDocumentReaderFactory;

    public BlogPostEditService(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        IDatabaseReaderFactory<BlogPostCreateDocumentReader> createDocumentReaderFactory,
        IDatabaseReaderFactory<BlogPostUpdateDocumentReader> updateDocumentReaderFactory,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdater> simpleTextNodeUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService,
        ILogger<BlogPostEditService> logger
    ): base(connection, siteDataService, nodeCacheService, simpleTextNodeUpdaterFactory, tagSaveService, tenantNodesSaveService, filesSaveService, textService, logger)
    {
        _createDocumentReaderFactory = createDocumentReaderFactory;
        _updateDocumentReaderFactory = updateDocumentReaderFactory;
    }

    protected override IEntityCreator<CreateModel.BlogPost> EntityCreator => new BlogPostCreator();

    public async Task<BlogPost> GetViewModelAsync(int userId, int tenantId)
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
    public async Task<BlogPost> GetViewModelAsync(int urlId, int userId, int tenantId)
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

    protected override CreateModel.BlogPost Map(BlogPost item)
    {
        var now = DateTime.Now;
        return new CreateModel.BlogPost {
            Id = null,
            Title = item.Title,
            Text = _textService.FormatText(item.Text),
            Teaser = _textService.FormatTeaser(item.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.BLOG_POST,
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