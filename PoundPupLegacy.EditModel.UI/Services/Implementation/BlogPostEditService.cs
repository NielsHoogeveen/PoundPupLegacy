using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class BlogPostEditService : SimpleTextNodeEditServiceBase<BlogPost, CreateModel.BlogPost>, IEditService<BlogPost>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, BlogPost> _createDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, BlogPost> _updateDocumentReaderFactory;
    private readonly IEntityCreator<CreateModel.BlogPost> _blogPostCreator;

    public BlogPostEditService(
        IDbConnection connection,
        ITenantRefreshService tenantRefreshService,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, BlogPost> createDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, BlogPost> updateDocumentReaderFactory,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest> simpleTextNodeUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService,
        IEntityCreator<CreateModel.BlogPost> blogPostCreator
    ) : base(connection, tenantRefreshService, simpleTextNodeUpdaterFactory, tagSaveService, tenantNodesSaveService, filesSaveService, textService)
    {
        _createDocumentReaderFactory = createDocumentReaderFactory;
        _updateDocumentReaderFactory = updateDocumentReaderFactory;
        _blogPostCreator = blogPostCreator;
    }

    protected sealed override IEntityCreator<CreateModel.BlogPost> EntityCreator => _blogPostCreator;

    public async Task<BlogPost?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
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
    public async Task<BlogPost?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _updateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
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

    protected sealed override CreateModel.BlogPost Map(BlogPost item)
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