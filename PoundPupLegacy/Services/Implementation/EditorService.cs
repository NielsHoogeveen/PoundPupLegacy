using Npgsql;
using PoundPupLegacy.Db;
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

    const string CTE_EDIT = $"""
        WITH
        {TENANT_NODES_DOCUMENT},
        {TENANTS_DOCUMENT},
        {DOCUMENTABLE_DOCUMENTS_DOCUMENT},
        {DOCUMENT_DOCUMENTABLES_DOCUMENT},
        {DOCUMENT_TYPES_DOCUMENT},
        {ATTACHMENTS_DOCUMENT},
        {ORGANIZATION_ORGANIZATION_TYPES_DOCUMENT},
        {ORGANIZATION_TYPES_DOCUMENT},
        {SUBDIVISIONS_DOCUMENT},
        {LOCATIONS_DOCUMENT},
        {COUNTRIES_DOCUMENT}
        """;

    const string CTE_CREATE = $"""
        WITH
        {TENANTS_DOCUMENT}
        """;


    const string DOCUMENT_TYPES_DOCUMENT = """
        document_types_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        n.id,
        		        'Name',
        		        n.title
        	        )
                ) document
            from document_type dt
            join term t on t.nameable_id = dt.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            join node n on n.id = dt.id 
            where tn.url_id = 42416 and tn.tenant_id = 1
        )
        """;

    const string ATTACHMENTS_DOCUMENT = """
        attachments_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        f.id,
                        'Name',
                        f.name,
        		        'Path',
                        f.path,
                        'Size',
                        f.size,
                        'MimeType',
                        f.mime_type,
                        'NodeId',
                        nf.node_id,
                        'HasBeenStored',
                        true
        	        )
                ) document
            from file f
            join node_file nf on nf.file_id = f.id
            join tenant_node tn on tn.node_id = nf.node_id
            where tn.url_id = @url_id and tn.tenant_id = @tenant_id
        )
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

    const string DOCUMENTABLE_DOCUMENTS_DOCUMENT = """
        documentable_documents_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'DocumentId',
        		        dd.document_id,
        		        'DocumentableId',
        		        dd.documentable_id,
        		        'Title',
        		        n.title
        	        )
                ) document
            from documentable_document dd
            join document d on d.id = dd.document_id
            join node n on n.id = d.id
            join tenant_node tn on tn.node_id = dd.documentable_id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;

    const string DOCUMENT_DOCUMENTABLES_DOCUMENT = """
        document_documentables_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'DocumentId',
        		        dd.document_id,
        		        'DocumentableId',
        		        dd.documentable_id,
        		        'Title',
        		        n.title
        	        )
                ) document
            from documentable_document dd
            join documentable d on d.id = dd.documentable_id
            join node n on n.id = d.id
            join tenant_node tn on tn.node_id = dd.document_id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
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
                    publication_status_id,
                    'HasBeenStored',
                    true,
                    'CanBeUnchecked',
                    false
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
                        'PublicationStatusIdDefault',
                        tenant_publication_status_id_default,
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
                    tenant_publication_status_id_default,
                    jsonb_agg(
        	            case when subgroup_id is null then null
        	            else 
        	            jsonb_build_object(
        		            'Id',
        		            subgroup_id,
        		            'Name',
        		            subgroup_name,
                            'PublicationStatusIdDefault',
                            subgroup_publication_status_id_default
        	            )
        	            end
                    ) subgroups
                from(
        		    select
        		        distinct
                        t.id,
                        t.domain_name,
                        pug.publication_status_id_default tenant_publication_status_id_default,
        		        s.name subgroup_name,
                        s.publication_status_id_default subgroup_publication_status_id_default,
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
                    join publishing_user_group pug on pug.id = t.id
                    left join(
                        select
                	        name,
                	        id,
                	        tenant_id,
                            publication_status_id_default
                        from(
                	        select
                		        distinct
                		        ug.name,
                		        s.id,
                                pug.publication_status_id_default,
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
                            join publishing_user_group pug on pug.id = s.id
                	        join user_group ug on ug.id = s.id
                        ) x
                        where allow_access =  true
                    ) s on s.tenant_id = t.id
                    join user_group ug on ug.id = t.id
                    join user_group_user_role_user uguru on uguru.user_group_id = ug.id
                    where uguru.user_id = @user_id
                ) x
                group by
                id, domain_name, allow_access,tenant_publication_status_id_default
        	) x
        )
        """;

    const string DOCUMENT_DOCUMENT = $"""
        {CTE_EDIT}
        select
            jsonb_build_object(
        	    'NodeId',
        	    d.id,
                'UrlId',
                tn.url_id,
                'PublisherId', 
                n.publisher_id,
                'OwnerId', 
                n.owner_id,
                'Title',
        	    n.title,
        	    'SourceUrl',
        	    d.source_url,
        	    'Text',
        	    d.text,
        	    'DocumentTypeId',
                case 
                    when d.document_type_id is null then 0
                    else d.document_type_id
                end,
        	    'PublicationDateFrom',
        	    lower(publication_date_range),
        	    'PublicationDateTo',
        	    upper(publication_date_range),
        	    'PublicationDate',
        	    publication_date,
                'DocumentableDocuments',
                (select document from document_documentables_document),
                'DocumentTypes',
                (select document from document_types_document),
                'TenantNodes',
                (select document from tenant_nodes_document),
                'Tenants',
                (select document from tenants_document),
                'Files',
                (select document from attachments_document)
            ) document
        from document d
        join node n on n.id = d.id
        join tenant_node tn on tn.node_id = d.id
        where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

    const string SIMPLE_TEXT_NODE_DOCUMENT = $"""
            {CTE_EDIT}
            select
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'UrlId', 
                    tn.url_id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title', 
                    n.title,
                    'Text', 
                    stn.text,
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
                    (select document from tenants_document),
                    'Files',
                    (select document from attachments_document)
                ) document
            from node n
            join simple_text_node stn on stn.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;

    const string ORGANIZATION_TYPES_DOCUMENT = """
        organization_types_document as (
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id',
                        ot.id,
                        'Name',
                        t.name
                    )
                ) document
            from organization_type ot
            join term t on t.nameable_id = ot.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 12622
        )
        """;

    const string ORGANIZATION_ORGANIZATION_TYPES_DOCUMENT = """
        organization_organization_types_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'OrganizationId',
        		        oot.organization_id,
        		        'OrganizationTypeId',
        		        oot.organization_type_id,
                        'Name',
                        t.name,
        		        'HasBeenStored',
        		        true,
        		        'HasBeenDeleted',
        		        false
        	        )
                ) document
            from organization_organization_type oot
            join tenant_node tn on tn.node_id = oot.organization_id
            join term t on t.nameable_id = oot.organization_type_id
            join tenant_node tn2 on tn2.node_id = t.vocabulary_id
            where tn2.tenant_id = 1 and tn2.url_id = 12622
            and tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;
    const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                jsonb_agg(jsonb_build_object(
        			'LocationId', "location_id",
                    'LocatableId', locatable_id,
        			'Street', street,
        			'Additional', additional,
        			'City', city,
        			'PostalCode', postal_code,
        			'SubdivisionId', subdivision_id,
                    'SubdivisionName', subdivision_name,
        			'CountryId', country_id,
                    'CountryName', country_name,
                    'Latitude', latitude,
                    'Longitude', longitude,
                    'Subdivisions', subdivisions
        		)) document
            from(
                select 
                ll.location_id,
                ll.locatable_id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                l.subdivision_id,
                s.name subdivision_name,
                l.country_id,
                nc.title country_name,
                l.latitude,
                l.longitude,
                (select document from subdivisions_document where country_id = l.country_id) subdivisions
                from "location" l
                join location_locatable ll on ll.location_id = l.id
                join node nc on nc.id = l.country_id
                left join subdivision s on s.id = l.subdivision_id
                join tenant_node tn on tn.node_id = ll.locatable_id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
                left join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = @tenant_id
                join tenant_node tn3 on tn3.node_id = nc.id and tn3.tenant_id = @tenant_id
            )x
        )
        """;

    const string COUNTRIES_DOCUMENT = $"""
        countries_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        c.id,
        		        'Name',
        		        t.name
        	        )
                ) document
            from country c
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
        )
        """;

    const string ORGANIZATION_DOCUMENT = $"""
            {CTE_EDIT}
            select
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'UrlId', 
                    tn.url_id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title' , 
                    n.title,
                    'Description', 
                    o.description,
                    'WebSiteUrl',
                    o.website_url,
                    'EmailAddress',
                    o.email_address,
                    'Established',
                    o.established,
                    'Terminated',
                    o.terminated,
                    'OrganizationOrganizationTypes',
                    (select document from organization_organization_types_document),
                    'OrganizationTypes',
                    (select document from organization_types_document),
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
                    (select document from tenants_document),
                    'Files',
                    (select document from attachments_document),
                    'Locations',
                    (select document from locations_document),
                    'Countries',
                    (select document from countries_document),
                    'Documents',
                    (select document from documentable_documents_document),
                    'Countries',
                    (select document from countries_document)
                ) document
            from node n
            join organization o on o.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
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
        return await GetNodeForEdit<BlogPost>(urlId, userId, tenantId, 35, SIMPLE_TEXT_NODE_DOCUMENT);
    }
    public async Task<Article?> GetArticle(int urlId, int userId, int tenantId)
    {
        return await GetNodeForEdit<Article>(urlId, userId, tenantId, 36, SIMPLE_TEXT_NODE_DOCUMENT);
    }
    public async Task<Discussion?> GetDiscussion(int urlId, int userId, int tenantId)
    {
        return await GetNodeForEdit<Discussion>(urlId, userId, tenantId, 37, SIMPLE_TEXT_NODE_DOCUMENT);
    }
    public async Task<Document?> GetDocument(int urlId, int userId, int tenantId)
    {
        return await GetNodeForEdit<Document>(urlId, userId, tenantId, 10, DOCUMENT_DOCUMENT);
    }
    public async Task<Organization?> GetOrganization(int urlId, int userId, int tenantId)
    {
        var res = await GetNodeForEdit<Organization>(urlId, userId, tenantId, 23, ORGANIZATION_DOCUMENT);
        return res;
    }

    private async Task<T?> GetNodeForEdit<T>(int url, int userId, int tenantId, int nodeTypeId, string sql)
        where T : class, Node
    {
        try {
            await _connection.OpenAsync();

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("node_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["url_id"].Value = url;
            readCommand.Parameters["user_id"].Value = userId;
            readCommand.Parameters["tenant_id"].Value = tenantId;
            readCommand.Parameters["node_type_id"].Value = nodeTypeId;
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows) {
                return null;
            }
            var text = reader.GetString(0); ;
            var blogPost = reader.GetFieldValue<T>(0);
            return blogPost;
        }
        finally {
            await _connection.CloseAsync();
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
