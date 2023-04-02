using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.CreateModel.Inserters;
using PoundPupLegacy.Deleters;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Inserters;
using PoundPupLegacy.Updaters;
using System.Data;
using System.Diagnostics;

namespace PoundPupLegacy.Services.Implementation;

public class EditorService : IEditorService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISiteDataService _siteDateService;
    private readonly INodeCacheService _nodeCacheService;
    private readonly ITextService _textService;
    private readonly ILogger<EditorService> _logger;
    private readonly IDatabaseReaderFactory<BlogPostCreateDocumentReader> _blogPostCreateDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<DiscussionCreateDocumentReader> _discussionCreateDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<BlogPostUpdateDocumentReader> _blogPostUpdateDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<DiscussionUpdateDocumentReader> _discussionUpdateDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<DocumentUpdateDocumentReader> _documentUpdateDocumentReaderFactory;
    private readonly IDatabaseReaderFactory<OrganizationUpdateDocumentReader> _organizationUpdateDocumentReaderFactory;
    private readonly IDatabaseDeleterFactory<FileDeleter> _fileDeleterFactory;
    private readonly IDatabaseDeleterFactory<TenantNodeDeleter> _tenantNodeDeleterFactory;
    private readonly IDatabaseDeleterFactory<NodeTermDeleter> _nodeTermDeleterFactory;
    private readonly IDatabaseUpdaterFactory<SimpleTextNodeUpdater> _simpleTextNodeUpdaterFactory;
    private readonly IDatabaseUpdaterFactory<TenantNodeUpdater> _tenantNodeUpdaterFactory;
    public EditorService(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        ITextService textService,
        ILogger<EditorService> logger,
        IDatabaseReaderFactory<BlogPostCreateDocumentReader> blogPostCreateDocumentReaderFactory,
        IDatabaseReaderFactory<DiscussionCreateDocumentReader> discussionCreateDocumentReaderFactory,
        IDatabaseReaderFactory<BlogPostUpdateDocumentReader> blogPostUpdateDocumentReaderFactory,
        IDatabaseReaderFactory<DiscussionUpdateDocumentReader> discussionUpdateDocumentReaderFactory,
        IDatabaseReaderFactory<DocumentUpdateDocumentReader> documentUpdateDocumentReaderFactory,
        IDatabaseReaderFactory<OrganizationUpdateDocumentReader> organizationUpdateDocumentReaderFactory,
        IDatabaseDeleterFactory<FileDeleter> fileDeleterFactory,
        IDatabaseDeleterFactory<TenantNodeDeleter> tenantNodeDeleterFactory,
        IDatabaseDeleterFactory<NodeTermDeleter> nodeTermDeleterFactory,
        IDatabaseUpdaterFactory<SimpleTextNodeUpdater> simpleTextNodeUpdaterFactory,
        IDatabaseUpdaterFactory<TenantNodeUpdater> tenantNodeUpdaterFactory
    )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _siteDateService = siteDataService;
        _nodeCacheService = nodeCacheService;
        _textService = textService;
        _logger = logger;
        _blogPostCreateDocumentReaderFactory = blogPostCreateDocumentReaderFactory;
        _discussionCreateDocumentReaderFactory = discussionCreateDocumentReaderFactory;
        _blogPostUpdateDocumentReaderFactory = blogPostUpdateDocumentReaderFactory;
        _discussionUpdateDocumentReaderFactory = discussionUpdateDocumentReaderFactory;
        _documentUpdateDocumentReaderFactory = documentUpdateDocumentReaderFactory;
        _organizationUpdateDocumentReaderFactory = organizationUpdateDocumentReaderFactory;
        _fileDeleterFactory = fileDeleterFactory;
        _tenantNodeDeleterFactory = tenantNodeDeleterFactory;
        _nodeTermDeleterFactory = nodeTermDeleterFactory;
        _tenantNodeUpdaterFactory = tenantNodeUpdaterFactory;
        _simpleTextNodeUpdaterFactory = simpleTextNodeUpdaterFactory;
    }

    public async Task<BlogPost?> GetNewBlogPost(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _blogPostCreateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeCreateDocumentRequest {
                NodeTypeId = Constants.BLOG_POST,
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
    public async Task<Discussion?> GetNewDiscussion(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _discussionCreateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeCreateDocumentRequest {
                NodeTypeId = Constants.DISCUSSION,
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


    public async Task<BlogPost?> GetBlogPost(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _blogPostUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<Discussion?> GetDiscussion(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _discussionUpdateDocumentReaderFactory.CreateAsync(_connection);
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
    public async Task<Document?> GetDocument(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentUpdateDocumentReaderFactory.CreateAsync(_connection);
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
    public async Task<Organization?> GetOrganization(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _organizationUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    private async Task Store(List<TenantNode> tenantNodes)
    {

        if (tenantNodes.Any(x => x.HasBeenDeleted)) {
            await using var deleter = await _tenantNodeDeleterFactory.CreateAsync(_connection);
            foreach (var tenantNode in tenantNodes.Where(x => x.HasBeenDeleted)) {
                if (tenantNode is not null && tenantNode.Id.HasValue) {
                    await deleter.DeleteAsync(tenantNode.Id.Value);
                }
            }
        }
        if (tenantNodes.Any(x => x.Id is null)) {
            await using var inserter = await TenantNodeInserter.CreateAsync(_connection);
                foreach (var tenantNode in tenantNodes.Where(x => !x.Id.HasValue)) {
                    var tenantNodeToCreate = new CreateModel.TenantNode {
                        Id = tenantNode.Id,
                        TenantId = tenantNode.TenantId,
                        NodeId = tenantNode.NodeId,
                        UrlId = tenantNode.UrlId,
                        UrlPath = tenantNode.UrlPath,
                        SubgroupId = tenantNode.SubgroupId,
                        PublicationStatusId = tenantNode.PublicationStatusId
                    };
                    await inserter.InsertAsync(tenantNodeToCreate);
                }
        }
        if (tenantNodes.Any(x => x.Id.HasValue)) {
            await using var updater = await _tenantNodeUpdaterFactory.CreateAsync(_connection);
            foreach (var tenantNode in tenantNodes.Where(x => x.Id.HasValue)) 
            {
                var tenantNodeUpdate = new TenantNodeUpdater.Request {
                    Id = tenantNode.Id!.Value,
                    UrlPath = tenantNode.UrlPath,
                    SubgroupId = tenantNode.SubgroupId,
                    PublicationStatusId = tenantNode.PublicationStatusId
                };
                await updater.UpdateAsync(tenantNodeUpdate);
            }
        }
    }

    private async Task Store(List<Tag> tags)
    {
        await using var deleter = await _nodeTermDeleterFactory.CreateAsync(_connection);
        foreach (var tag in tags.Where(x => x.HasBeenDeleted)) {
            await deleter.DeleteAsync(new NodeTermDeleter.Request 
            { 
                NodeId = tag.NodeId!.Value, 
                TermId = tag.TermId 
            });
        }
        
        await using var inserter = await NodeTermInserter.CreateAsync(_connection);
        foreach (var tag in tags.Where(x => !x.IsStored)) {
            await inserter.InsertAsync(new CreateModel.NodeTerm {
                NodeId = tag.NodeId!.Value,
                TermId = tag.TermId
            });
        }
    }
    private async Task Store(SimpleTextNode node)
    {
        await using var updater = await _simpleTextNodeUpdaterFactory.CreateAsync(_connection);
        await updater.UpdateAsync(new SimpleTextNodeUpdater.Request 
        {
            Title = node.Title,
            Text = _textService.FormatText(node.Text),
            Teaser = _textService.FormatTeaser(node.Text),
            NodeId = node.NodeId!.Value
        });
    }
    private async Task Store(List<EditModel.File> attachments)
    {

        if (attachments.Any(x => x.HasBeenDeleted)) {
            await using var deleter = await _fileDeleterFactory.CreateAsync(_connection);
            foreach (var attachment in attachments.Where(x => x.HasBeenDeleted)) {
                await deleter.DeleteAsync(new FileDeleter.Request {
                    FileId = attachment.Id!.Value,
                    NodeId = attachment.NodeId!.Value
                });
            }
        }
        if (attachments.Any(x => x.Id is null)) {
            await using var inserter = await FileInserter.CreateAsync(_connection);
            foreach (var attachment in attachments.Where(x => x.Id is null)) {
                await inserter.InsertAsync(new FileInserter.Request {
                    MimeType = attachment.MimeType,
                    Path = attachment.Path,
                    Size = attachment.Size,
                    Name = attachment.Name,
                    NodeId= attachment.NodeId!.Value
                });
            }
        }
    }

    private async Task StoreNew(SimpleTextNode stn)
    {
        switch (stn) {
            case BlogPost bp:
                await StoreNewBlogPost(bp);
                break;
            case Article a:
                await StoreNewArticle(a);
                break;
            case Discussion d:
                await StoreNewDiscussion(d);
                break;
        };
        foreach (var topic in stn.Tags) {
            topic.NodeId = stn.UrlId;
        }
        await Store(stn.Tags);
        foreach (var file in stn.Files) {
            file.NodeId = stn.UrlId;
        }
        await Store(stn.Files);

    }
    private async Task StoreNewBlogPost(BlogPost blogPost)
    {
        var now = DateTime.Now;
        var nodeToStore = new CreateModel.BlogPost {
            Id = null,
            Title = blogPost.Title,
            Text = _textService.FormatText(blogPost.Text),
            Teaser = _textService.FormatTeaser(blogPost.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 35,
            OwnerId = blogPost.OwnerId,
            PublisherId = blogPost.PublisherId,
            TenantNodes = blogPost.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
        var blogPosts = new List<CreateModel.BlogPost> { nodeToStore };
        await new BlogPostCreator().CreateAsync(blogPosts.ToAsyncEnumerable(), _connection);
        blogPost.UrlId = nodeToStore.Id;
    }
    private async Task StoreNewArticle(Article article)
    {
        var now = DateTime.Now;
        var nodeToStore = new CreateModel.Article {
            Id = null,
            Title = article.Title,
            Text = _textService.FormatText(article.Text),
            Teaser = _textService.FormatTeaser(article.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 36,
            OwnerId = article.OwnerId,
            PublisherId = article.PublisherId,
            TenantNodes = article.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
        var blogPosts = new List<CreateModel.Article> { nodeToStore };
        await new ArticleCreator().CreateAsync(blogPosts.ToAsyncEnumerable(), _connection);
        article.UrlId = nodeToStore.Id;
    }
    private async Task StoreNewDiscussion(Discussion discussion)
    {
        var now = DateTime.Now;
        var nodeToStore = new CreateModel.Discussion {
            Id = null,
            Title = discussion.Title,
            Text = _textService.FormatText(discussion.Text),
            Teaser = _textService.FormatTeaser(discussion.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 37,
            OwnerId = discussion.OwnerId,
            PublisherId = discussion.PublisherId,
            TenantNodes = discussion.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
        var blogPosts = new List<CreateModel.Discussion> { nodeToStore };
        await new DiscussionCreator().CreateAsync(blogPosts.ToAsyncEnumerable(), _connection);
        discussion.UrlId = nodeToStore.Id;
    }

    public async Task Save(SimpleTextNode post)
    {
        var sp = new Stopwatch();
        sp.Start();
        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        _logger.LogInformation($"Started transaction in {sp.ElapsedMilliseconds}");
        try {
            if (post.NodeId.HasValue) {
                await Store(post);
                _logger.LogInformation($"Stored simple text node after {sp.ElapsedMilliseconds}");
                await Store(post.Tags);
                _logger.LogInformation($"Stored tags after {sp.ElapsedMilliseconds}");
                await Store(post.Tenants.Where(x => x.TenantNode is not null).Select(x => x.TenantNode!).ToList());
                _logger.LogInformation($"Stored tenant nodes after {sp.ElapsedMilliseconds}");
                await Store(post.Files);
                _logger.LogInformation($"Stored tenant attachments after {sp.ElapsedMilliseconds}");
            }
            else {
                await StoreNew(post);
                _logger.LogInformation($"Stored new blogpost after {sp.ElapsedMilliseconds}");
            }
            tx.Commit();
            _logger.LogInformation($"Committed after {sp.ElapsedMilliseconds}");
            if (post.UrlId.HasValue) {
                _nodeCacheService.Remove(post.UrlId.Value, post.OwnerId);
            }
            _logger.LogInformation($"Removed from cache after {sp.ElapsedMilliseconds}");
            await _siteDateService.RefreshTenants();
            _logger.LogInformation($"Refreshed tenant data after {sp.ElapsedMilliseconds}");
        }
        catch (Exception) {
            tx.Rollback();
            throw;
        }
        finally {
            await _connection.CloseAsync();
        }

    }
    public async Task Save(Document document)
    {
        await Task.CompletedTask;
    }
    public async Task Save(Organization organization)
    {
        await Task.CompletedTask;
    }

}
