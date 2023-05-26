﻿namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DeportationCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<ExistingDeportationCase>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DEPORTATION_CASE;

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
                    'SubdivisionFrom',
                    case
                        when n2.id is null then null
                        else jsonb_build_object(
                            'Id', 
                            n2.id,
                            'Name', 
                            n2.title
                        )
                    end,
                    'CountryTo',
                    case
                        when n1.id is null then null
                        else jsonb_build_object(
                            'Id', 
                            n1.id,
                            'Name', 
                            n1.title
                        )
                    end,
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
            join organization o on o.id = n.id
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join deportation_case dc on dc.id = c.id
            join tenant_node tn on tn.node_id = n.id
            left join node n1 on n1.id = dc.country_id_to
            left join node n2 on n2.id = dc.subdivision_id_from
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id 
        """;

}
