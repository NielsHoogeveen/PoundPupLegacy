namespace PoundPupLegacy.EditModel.Readers;

internal sealed class ChildTraffickingCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<ExistingChildTraffickingCase>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.CHILD_TRAFFICKING_CASE;

    const string SQL = $"""
            {CTE_EDIT},    
            {SharedSql.CASE_CASE_PARTY_DOCUMENT}
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
                    'Date',
                    c.fuzzy_date,
                    'CountryFrom',
                    jsonb_build_object(
                        'Id',
                        n.id,
                        'Name',
                        n.title
                    ),
                    'NumberOfChildrenInvolved',
                    ctc.number_of_children_involved,
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
                    'CasePartyTypesCaseParties',
                    (select document from case_case_party_document)
                ) document
            from node n
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join child_trafficking_case ctc on ctc.id = c.id
            join tenant_node tn on tn.node_id = n.id
            join node n2 on n2.id = ctc.country_id_from
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

}
