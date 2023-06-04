namespace PoundPupLegacy.EditModel.Readers;

public abstract class SimpleTextNodeUpdateDocumentReaderFactory<TResponse> : NodeUpdateDocumentReaderFactory<TResponse>
where TResponse : class, SimpleTextNode, ExistingNode
{
    public override string Sql => SQL;
    protected const string SQL = $"""
            {SharedSql.NODE_UPDATE_CTE}
            select
                jsonb_build_object(
                    'NodeIdentification', 
                    (select document from identification_document where id = n.id),
                    'NodeDetailsForUpdate',
                    (select document from node_details_document where id = n.id),
                    'SimpleTextNodeDetails',
                    json_build_object(
                        'Text', 
                        stn.text
                    )
                ) document
            from node n
            join simple_text_node stn on stn.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;
}

