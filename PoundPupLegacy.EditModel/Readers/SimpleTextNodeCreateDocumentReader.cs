namespace PoundPupLegacy.EditModel.Readers;

public abstract class SimpleTextNodeCreateDocumentReaderFactory<TResponse> : NodeCreateDocumentReaderFactory<TResponse>
where TResponse : class, SimpleTextNode, NewNode
{
    public override string Sql => SQL;
    private const string SQL = $"""
            {SharedSql.NODE_CREATE_CTE}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'SimpleTextNodeDetails',
                    json_build_object(
                        'Text',
                        ''
                    )
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;

}

