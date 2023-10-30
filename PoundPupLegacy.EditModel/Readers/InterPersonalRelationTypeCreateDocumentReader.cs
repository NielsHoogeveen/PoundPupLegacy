namespace PoundPupLegacy.EditModel.Readers;

internal sealed class InterPersonalRelationTypeCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<InterPersonalRelationType.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.INTER_PERSONAL_RELATION_TYPE;

    private const string SQL = $"""
            {SharedSql.NAMEABLE_CREATE_CTE}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'NameableDetails',
                    (select document from nameable_details_document),
                    'IsSymmetric',
                    false
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
