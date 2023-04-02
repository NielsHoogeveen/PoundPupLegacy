using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Updaters;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class ArticleEditService : SimpleTextNodeEditServiceBase<Article, CreateModel.Article>, IEditService<Article>
{
    private readonly IDatabaseReaderFactory<ArticleCreateDocumentReader> _articleCreateDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<ArticleUpdateDocumentReader> _articleUpdateDocumentReaderFactory;

    public ArticleEditService(
        IDbConnection connection,
        IDatabaseReaderFactory<ArticleCreateDocumentReader> articleCreateDocumentReaderFactory,
        IDatabaseReaderFactory<ArticleUpdateDocumentReader> articleUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdater> simpleTextNodeUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ITextService textService
    ): base(connection, simpleTextNodeUpdaterFactory, tagSaveService, textService)
    {
        _articleCreateDocumentReaderFactory = articleCreateDocumentReaderFactory;
        _articleUpdateDocumentReaderFactory = articleUpdateDocumentReaderFactory;
    }

    protected override IEntityCreator<CreateModel.Article> EntityCreator => new ArticleCreator();

    public async Task<Article> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _articleCreateDocumentReaderFactory.CreateAsync(_connection);
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
    public async Task<Article> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _articleUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    protected override CreateModel.Article Map(Article item)
    {
        var now = DateTime.Now;
        return new CreateModel.Article {
            Id = null,
            Title = item.Title,
            Text = _textService.FormatText(item.Text),
            Teaser = _textService.FormatTeaser(item.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 36,
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