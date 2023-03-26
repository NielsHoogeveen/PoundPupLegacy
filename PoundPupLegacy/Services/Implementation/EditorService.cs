using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
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
    public EditorService(
    NpgsqlConnection connection,
    ISiteDataService siteDataService,
    INodeCacheService nodeCacheService,
    ITextService textService,
    ILogger<EditorService> logger)
    {
        _connection = connection;
        _siteDateService = siteDataService;
        _nodeCacheService = nodeCacheService;
        _textService = textService;
        _logger = logger;
    }

    const string CTE_CREATE = $"""
        WITH
        """;


    const string SUBDIVISIONS_DOCUMENT = """
        subdivisions_document as(
            select
            c.country_id,
            jsonb_agg(
        	    jsonb_build_object(
        		    'Id',
        		    c.id,
        		    'Name',
        		    t.name
        	    )
            ) document
            from subdivision c
            join bottom_level_subdivision b on b.id = c.id
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
            group by c.country_id
        )
        """;

    const string NEW_SIMPLE_TEXT_DOCUMENT = $"""
            {CTE_CREATE}
            select
                jsonb_build_object(
                    'NodeId', 
                    null,
                    'UrlId', 
                    null,
                    'PublisherId',
                    @user_id,
                    'OwnerId',
                    @tenant_id,
                    'Title', 
                    '',
                    'Text', 
                    '',
            		'Tags', null,
                    'TenantNodes',
                    null,
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null
                ) document
        """;

    public async Task<IEnumerable<SubdivisionListItem>> GetSubdivisions(int countryId)
    {
        try {
            await _connection.OpenAsync();
            var sql = SUBDIVISIONS_DOCUMENT;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("country_id", NpgsqlTypes.NpgsqlDbType.Integer);
           await readCommand.PrepareAsync();
            readCommand.Parameters["country_id"].Value = countryId;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows) {
                return new List<SubdivisionListItem>();
            }
            var text = reader.GetString(0); ;
            var subdivisions = reader.GetFieldValue<List<SubdivisionListItem>>(0);
            return subdivisions;
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    public async Task<BlogPost?> GetNewBlogPost(int userId, int tenantId)
    {
        return await GetNewSimpleTextNode<BlogPost>(35, userId, tenantId);
    }
    public async Task<Article?> GetNewArticle(int userId, int tenantId)
    {
        return await GetNewSimpleTextNode<Article>(36, userId, tenantId);
    }
    public async Task<Discussion?> GetNewDiscussion(int userId, int tenantId)
    {
        return await GetNewSimpleTextNode<Discussion>(37, userId, tenantId);
    }

    public async Task<T?> GetNewSimpleTextNode<T>(int nodeTypeId, int userId, int tenantId)
        where T : class, SimpleTextNode
    {
        try {
            await _connection.OpenAsync();
            var sql = NEW_SIMPLE_TEXT_DOCUMENT;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("node_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["tenant_id"].Value = tenantId;
            readCommand.Parameters["node_type_id"].Value = nodeTypeId;
            readCommand.Parameters["user_id"].Value = userId;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows) {
                return null;
            }
            var text = reader.GetString(0); ;
            var node = reader.GetFieldValue<T>(0);
            return node;
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    public async Task<BlogPost?> GetBlogPost(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await BlogPostEditDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeEditDocumentRequest {
                UrlId = urlId, 
                UserId = userId, 
                TenantId = tenantId

            });
        }
        finally { 
            if(_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }

    }
    public async Task<Article?> GetArticle(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await ArticleEditDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeEditDocumentRequest {
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
            await using var reader = await DiscussionEditDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeEditDocumentRequest { 
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
            await using var reader = await DocumentEditDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeEditDocumentRequest { 
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
            await using var reader = await OrganizationEditDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeEditDocumentRequest { 
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
            using (var command = _connection.CreateCommand()) {
                var sql = $"""
                    delete from tenant_node
                    where id = @id;
                    """;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;
                command.CommandText = sql;
                command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
                await command.PrepareAsync();
                foreach (var tenantNode in tenantNodes.Where(x => x.HasBeenDeleted)) {
                    command.Parameters["id"].Value = tenantNode.Id;
                    var u = await command.ExecuteNonQueryAsync();
                }
            }
        }
        if (tenantNodes.Any(x => x.Id is null)) {
            using (var command = _connection.CreateCommand()) {
                var sql = $"""
                    insert into tenant_node (node_id, tenant_id, url_path, url_id, subgroup_id, publication_status_id) VALUES(@node_id, @tenant_id, @url_path, @url_id, @subgroup_id, @publication_status_id)
                    """;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;
                command.CommandText = sql;
                command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("url_path", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("subgroup_id", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("publication_status_id", NpgsqlTypes.NpgsqlDbType.Integer);
                await command.PrepareAsync();
                foreach (var tenantNode in tenantNodes.Where(x => !x.Id.HasValue)) {
                    command.Parameters["node_id"].Value = tenantNode.NodeId;
                    command.Parameters["tenant_id"].Value = tenantNode.TenantId;
                    if (tenantNode.UrlPath is null) {
                        command.Parameters["url_path"].Value = DBNull.Value;
                    }
                    else {
                        command.Parameters["url_path"].Value = tenantNode.UrlPath;
                    }
                    command.Parameters["url_id"].Value = tenantNode.UrlId;
                    if (tenantNode.SubgroupId.HasValue) {
                        command.Parameters["subgroup_id"].Value = tenantNode.SubgroupId;
                    }
                    else {
                        command.Parameters["subgroup_id"].Value = DBNull.Value;
                    }
                    command.Parameters["publication_status_id"].Value = tenantNode.PublicationStatusId;
                    var u = await command.ExecuteNonQueryAsync();
                }
            }
        }
        if (tenantNodes.Any(x => x.Id.HasValue)) {
            using (var command = _connection.CreateCommand()) {
                var sql = $"""
                    update tenant_node 
                    set 
                    url_path = @url_path, 
                    subgroup_id = @subgroup_id, 
                    publication_status_id = @publication_status_id
                    where id = @id
                    """;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;
                command.CommandText = sql;
                command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("url_path", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("subgroup_id", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("publication_status_id", NpgsqlTypes.NpgsqlDbType.Integer);
                await command.PrepareAsync();
                foreach (var tenantNode in tenantNodes.Where(x => x.Id.HasValue)) {
                    command.Parameters["id"].Value = tenantNode.Id!;
                    if (string.IsNullOrEmpty(tenantNode.UrlPath)) {
                        command.Parameters["url_path"].Value = DBNull.Value;
                    }
                    else {
                        command.Parameters["url_path"].Value = tenantNode.UrlPath;
                    }
                    if (tenantNode.SubgroupId.HasValue) {
                        command.Parameters["subgroup_id"].Value = tenantNode.SubgroupId;
                    }
                    else {
                        command.Parameters["subgroup_id"].Value = DBNull.Value;
                    }
                    command.Parameters["publication_status_id"].Value = tenantNode.PublicationStatusId;
                    var u = await command.ExecuteNonQueryAsync();
                }
            }
        }

    }

    private async Task Store(List<Tag> tags)
    {
        using (var command = _connection.CreateCommand()) {
            var sql = $"""
                    delete from node_term
                    where node_id = @node_id and term_id = @term_id;
                    """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("term_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            foreach (var tag in tags.Where(x => x.HasBeenDeleted)) {
                command.Parameters["node_id"].Value = tag.NodeId;
                command.Parameters["term_id"].Value = tag.TermId;
                var u = await command.ExecuteNonQueryAsync();
            }

        }
        using (var command = _connection.CreateCommand()) {
            var sql = $"""
                    insert into node_term (node_id, term_id) VALUES(@node_id, @term_id)
                    """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("term_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            foreach (var tag in tags.Where(x => !x.IsStored)) {
                command.Parameters["node_id"].Value = tag.NodeId;
                command.Parameters["term_id"].Value = tag.TermId;
                var u = await command.ExecuteNonQueryAsync();
            }
        }

    }
    private async Task Store(SimpleTextNode node)
    {
        using (var command = _connection.CreateCommand()) {
            var sql = $"""
                    update node set title=@title
                    where id = @node_id;
                    update simple_text_node set text=@text, teaser=@teaser
                    where id = @node_id;
                    """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            command.Parameters.Add("text", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("teaser", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("title", NpgsqlTypes.NpgsqlDbType.Varchar);
            command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["title"].Value = node.Title;
            command.Parameters["text"].Value = _textService.FormatText(node.Text);
            command.Parameters["teaser"].Value = _textService.FormatTeaser(node.Text);
            command.Parameters["node_id"].Value = node.NodeId;
            var u = await command.ExecuteNonQueryAsync();
        }
    }
    private async Task Store(List<EditModel.File> attachments)
    {
        if (attachments.Any(x => x.HasBeenDeleted)) {
            var command = _connection.CreateCommand();
            var sql = $"""
                delete from node_file
                where file_id = @file_id and node_id = @node_id;
                delete from tenant_file
                where file_id in (
                    select 
                    id 
                    from file f
                    left join node_file nf on nf.file_id = f.id
                    where nf.file_id is null
                    and f.id = @file_id
                );
                delete from file
                where id in (
                    select 
                    id 
                    from file f
                    left join node_file nf on nf.file_id = f.id
                    where nf.file_id is null
                    and f.id = @file_id
                );
                """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            command.Parameters.Add("file_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            foreach (var attachment in attachments.Where(x => x.HasBeenDeleted)) {
                command.Parameters["file_id"].Value = attachment.Id;
                command.Parameters["node_id"].Value = attachment.NodeId;
                await command.ExecuteNonQueryAsync();
            }
        }
        if (attachments.Any(x => x.Id is null)) {
            using (var command = _connection.CreateCommand()) {
                var sql = $"""
                    insert into file (name, size, mime_type, path) VALUES(@name, @size, @mime_type, @path);
                    insert into node_file (node_id, file_id) VALUES(@node_id, lastval());
                    """;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;
                command.CommandText = sql;
                command.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("size", NpgsqlTypes.NpgsqlDbType.Integer);
                command.Parameters.Add("mime_type", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("path", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
                await command.PrepareAsync();
                foreach (var attachment in attachments.Where(x => !x.HasBeenStored)) {
                    command.Parameters["name"].Value = attachment.Name;
                    command.Parameters["size"].Value = attachment.Size;
                    command.Parameters["mime_type"].Value = attachment.MimeType;
                    command.Parameters["path"].Value = attachment.Path;
                    command.Parameters["node_id"].Value = attachment.NodeId;
                    await command.ExecuteNonQueryAsync();
                }
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
        var nodeToStore = new Model.BlogPost {
            Id = null,
            Title = blogPost.Title,
            Text = _textService.FormatText(blogPost.Text),
            Teaser = _textService.FormatTeaser(blogPost.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 35,
            OwnerId = blogPost.OwnerId,
            PublisherId = blogPost.PublisherId,
            TenantNodes = blogPost.Tenants.Where(t => t.HasTenantNode).Select(tn => new Model.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
        var blogPosts = new List<Model.BlogPost> { nodeToStore };
        await BlogPostCreator.CreateAsync(blogPosts.ToAsyncEnumerable(), _connection);
        blogPost.UrlId = nodeToStore.Id;
    }
    private async Task StoreNewArticle(Article article)
    {
        var now = DateTime.Now;
        var nodeToStore = new Model.Article {
            Id = null,
            Title = article.Title,
            Text = _textService.FormatText(article.Text),
            Teaser = _textService.FormatTeaser(article.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 36,
            OwnerId = article.OwnerId,
            PublisherId = article.PublisherId,
            TenantNodes = article.Tenants.Where(t => t.HasTenantNode).Select(tn => new Model.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
        var blogPosts = new List<Model.Article> { nodeToStore };
        await ArticleCreator.CreateAsync(blogPosts.ToAsyncEnumerable(), _connection);
        article.UrlId = nodeToStore.Id;
    }
    private async Task StoreNewDiscussion(Discussion discussion)
    {
        var now = DateTime.Now;
        var nodeToStore = new Model.Discussion {
            Id = null,
            Title = discussion.Title,
            Text = _textService.FormatText(discussion.Text),
            Teaser = _textService.FormatTeaser(discussion.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = 37,
            OwnerId = discussion.OwnerId,
            PublisherId = discussion.PublisherId,
            TenantNodes = discussion.Tenants.Where(t => t.HasTenantNode).Select(tn => new Model.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
        var blogPosts = new List<Model.Discussion> { nodeToStore };
        await DiscussionCreator.CreateAsync(blogPosts.ToAsyncEnumerable(), _connection);
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
