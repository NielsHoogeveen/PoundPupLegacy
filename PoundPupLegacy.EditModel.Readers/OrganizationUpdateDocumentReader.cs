using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class OrganizationUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<OrganizationUpdateDocumentReader>
{
    public override async Task<OrganizationUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new OrganizationUpdateDocumentReader(command);
    }

    const string SQL = $"""
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

}

public class OrganizationUpdateDocumentReader : NodeUpdateDocumentReader<Organization>
{
    internal OrganizationUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ORGANIZATION)
    {
    }
}


