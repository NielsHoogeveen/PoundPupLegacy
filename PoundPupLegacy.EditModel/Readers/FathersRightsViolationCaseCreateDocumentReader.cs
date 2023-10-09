namespace PoundPupLegacy.EditModel.Readers;

internal sealed class FathersRightsViolationCaseCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<FathersRightsViolationCase.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.FATHERS_RIGHTS_VIOLATION_CASE;

    private const string SQL = $"""
            {SharedSql.CASE_CREATE_CTE}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'NameableDetails',
                    (select document from nameable_details_document),
                    'LocatableDetailsForCreate',
                    (select document from locatable_details_document),
                    'CaseDetails',
                    (select document from case_details_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
