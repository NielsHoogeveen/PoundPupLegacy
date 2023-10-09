namespace PoundPupLegacy.EditModel.Readers;

internal sealed class ChildTraffickingCaseCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<UnresolvedChildTraffickingCase.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.CHILD_TRAFFICKING_CASE;

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
                    (select document from case_details_document),
                    'NewChildTraffickingCaseDetails',
                    jsonb_build_object(
                        'NumberOfChildrenInvolved',
                        null,
                        'CountryFromNew',
                        null
                    )
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
