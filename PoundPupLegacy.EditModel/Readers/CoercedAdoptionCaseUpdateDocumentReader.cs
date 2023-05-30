namespace PoundPupLegacy.EditModel.Readers;

internal sealed class CoercedAdoptionCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<CoercedAdoptionCase.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.COERCED_ADOPTION_CASE;

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
                    'VocabularyIdTagging',
                    (select id from tagging_vocabulary),
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
            join coerced_adoption_case coc on coc.id = c.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

}
