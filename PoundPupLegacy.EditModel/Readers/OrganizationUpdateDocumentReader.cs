namespace PoundPupLegacy.EditModel.Readers;

internal sealed class OrganizationUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Organization>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ORGANIZATION;

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
                    nm.description,
                    'WebSiteUrl',
                    o.website_url,
                    'EmailAddress',
                    o.email_address,
                    'EstablishmentDateFrom',
                    lower(o.established),
                    'EstablishmentDateTo',
                    upper(o.established),
                    'TerminationDateFrom',
                    lower(o.terminated),
                    'TerminationDateTo',
                    upper(o.terminated),
                    'OrganizationOrganizationTypes',
                    (select document from organization_organization_types_document),
                    'OrganizationTypes',
                    (select document from organization_types_document),
            		'Tags', 
                    (select document from tags_document),
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
                    'Countries',
                    (select document from countries_document)
                ) document
            from node n
            join organization o on o.id = n.id
            join nameable nm on nm.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;

}
