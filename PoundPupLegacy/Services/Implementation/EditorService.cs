using Npgsql;
using PoundPupLegacy.EditModel;
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

    const string CTE = $"""
        WITH
        {TENANT_NODES_DOCUMENT},
        {TENANTS_DOCUMENT}
        """;

    const string TENANT_NODES_DOCUMENT = """
        tenant_nodes_document as(
        select
            jsonb_agg(
                jsonb_build_object(
                    'Id',
                    id,
                    'TenantId',
                    tenant_id,
                    'UrlId',
                    url_id,
                    'UrlPath',
                    url_path,
                    'NodeId',
                    node_id,
                    'SubgroupId',
                    subgroup_id,
                    'PublicationStatusId',
                    publication_status_id
                )
            ) document
        from(
            select
        		tn2.id,
        		tn2.tenant_id,
        		tn2.url_id,
        		tn2.url_path,
        		tn2.node_id,
        		tn2.subgroup_id,
        		tn2.publication_status_id,
        		(
                    select
                        case 
                            when max(status) = 1 then true
                            else false
                        end
                    from(
                        select
                            distinct
                            1 status
                        from user_group_user_role_user uguru
                        join user_group ug on ug.id = uguru.user_group_id
                        where uguru.user_id = @user_id
                        and user_group_id = tn2.tenant_id
                        and ug.administrator_role_id = uguru.user_role_id
                        union
                        select
                            distinct
                            1 status
                        from user_group_user_role_user uguru
                        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                        join create_node_action cna on cna.id = arp.action_id
                        where uguru.user_id = @user_id
                        and user_group_id = tn2.tenant_id
                        and cna.node_type_id = @node_type_id
                    ) x
                ) allow_access
                from tenant_node tn
                join tenant_node tn2 on tn2.node_id = tn.node_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
        ) x 
        where allow_access = true
        )
        """;
    const string TENANTS_DOCUMENT = """
        tenants_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        id,
        		        'DomainName',
        		        domain_name,
        		        'AllowAccess',
        		        allow_access,
        		        'Subgroups',
        		        case 
        			        when subgroups = '[null]' then null
        			        else subgroups
        		        end
        	        )
                ) document
            from(
                select
                    id,
                    domain_name,
                    allow_access,
                    jsonb_agg(
        	            case when subgroup_id is null then null
        	            else 
        	            jsonb_build_object(
        		            'Id',
        		            subgroup_id,
        		            'Name',
        		            subgroup_name
        	            )
        	            end
                    ) subgroups
                from(
        		    select
        		        distinct
                        t.id,
                        t.domain_name,
        		        s.name subgroup_name,
        		        s.id subgroup_id,
                        (
                	        select
                		        case 
                			        when max(status) = 1 then true
                			        else false
                		        end
                	        from(
                		        select
                			        distinct
                			        1 status
                		        from user_group_user_role_user uguru
                		        join user_group ug on ug.id = uguru.user_group_id
                		        where uguru.user_id = @user_id
                		        and user_group_id = t.id
                		        and ug.administrator_role_id = uguru.user_role_id
                		        union
                		        select
                			        distinct
                			        1 status
                		        from user_group_user_role_user uguru
                		        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                		        join create_node_action cna on cna.id = arp.action_id
                		        where uguru.user_id = @user_id
                		        and user_group_id = t.id
                		        and cna.node_type_id = @node_type_id
                	        ) x
                        ) allow_access
                    from tenant t
                    left join(
                        select
                	        name,
                	        id,
                	        tenant_id
                        from(
                	        select
                		        distinct
                		        ug.name,
                		        s.id,
                		        s.tenant_id,
                		        (
                			        select
                				        case 
                					        when max(status) = 1 then true
                					        else false
                				        end
                			        from(
                				        select
                					        distinct
                					        1 status
                				        from user_group_user_role_user uguru
                				        join user_group ug on ug.id = uguru.user_group_id
                				        where uguru.user_id = @user_id
                				        and user_group_id = s.id
                				        and ug.administrator_role_id = uguru.user_role_id
                				        union
                				        select
                					        distinct
                					        1 status
                				        from user_group_user_role_user uguru
                				        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                				        join create_node_action cna on cna.id = arp.action_id
                				        where uguru.user_id = @user_id
                				        and user_group_id = s.id
                				        and cna.node_type_id = @node_type_id
                			        ) x
                		        ) allow_access
                	        from subgroup s 
                	        join user_group ug on ug.id = s.id
                        ) x
                        where allow_access =  true
                    ) s on s.tenant_id = t.id
                    join user_group ug on ug.id = t.id
                    join user_group_user_role_user uguru on uguru.user_group_id = ug.id
                    where uguru.user_id = @user_id
                ) x
                group by
                id, domain_name, allow_access
        	) x
        )
        """;

    public async Task<BlogPost?> GetBlogPost(int id)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            {CTE}
            select
                jsonb_build_object(
                    'NodeId', n.id,
                    'UrlId', tn.url_id,
                    'Title' , n.title,
                    'Text', stn.text,
            		'Tags', (
            			select 
            			jsonb_agg(jsonb_build_object(
            				'NodeId', tn.node_id,
            				'TermId', t.id,
            				'Name', t.name
            			))
            			from node_term nt
            			join tenant tt on tt.id = @tenant_id
            			join term t on t.id = nt.term_id and t.vocabulary_id = tt.vocabulary_id_tagging
            			join tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id
            			where nt.node_id = n.id
            		),
                    'TenantNodes',
                    (select document from tenant_nodes_document),
                    'Tenants',
                    (select document from tenants_document)
                ) document
            from node n
            join blog_post b on b.id = n.id
            join simple_text_node stn on stn.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("node_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["url_id"].Value = id;
            readCommand.Parameters["tenant_id"].Value = _siteDateService.GetTenantId();
            readCommand.Parameters["node_type_id"].Value = 35;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows)
            {
                return null;
            }
            var text = reader.GetString(0); ;
            var blogPost = reader.GetFieldValue<BlogPost>(0);
            return blogPost;
        }
        finally
        {
            await _connection.CloseAsync();
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
        using (var command = _connection.CreateCommand())
        {
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
            foreach (var tag in tags.Where(x => x.HasBeenDeleted))
            {
                command.Parameters["node_id"].Value = tag.NodeId;
                command.Parameters["term_id"].Value = tag.TermId;
                var u = await command.ExecuteNonQueryAsync();
            }

        }
        using (var command = _connection.CreateCommand())
        {
            var sql = $"""
                    insert into node_term (node_id, term_id) VALUES(@node_id, @term_id)
                    """;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = sql;
            command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("term_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            foreach (var tag in tags.Where(x => !x.IsStored))
            {
                command.Parameters["node_id"].Value = tag.NodeId;
                command.Parameters["term_id"].Value = tag.TermId;
                var u = await command.ExecuteNonQueryAsync();
            }
        }

    }

    public async Task Store(SimpleTextNode node)
    {
        using (var command = _connection.CreateCommand())
        {
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

    public async Task Save(BlogPost post)
    {
        var sp = new Stopwatch();
        sp.Start();
        await _connection.OpenAsync();
        var tx = await _connection.BeginTransactionAsync();
        _logger.LogInformation($"Started transaction in {sp.ElapsedMilliseconds}");
        try
        {
            await Store(post);
            _logger.LogInformation($"Stored blogpost after {sp.ElapsedMilliseconds}");
            await Store(post.Tags);
            _logger.LogInformation($"Stored tags after {sp.ElapsedMilliseconds}");
            await Store(post.Tenants.Where(x => x.TenantNode is not null).Select(x => x.TenantNode!).ToList());
            _logger.LogInformation($"Stored tenant nodes {sp.ElapsedMilliseconds}");
            tx.Commit();
            _logger.LogInformation($"Committed after {sp.ElapsedMilliseconds}");
            _nodeCacheService.Remove(post.UrlId);
            _logger.LogInformation($"Removed from cache after {sp.ElapsedMilliseconds}");
            await _siteDateService.RefreshTenants();
            _logger.LogInformation($"Refreshed tenant data after {sp.ElapsedMilliseconds}");
        }
        catch (Exception)
        {
            tx.Rollback();
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }
}
