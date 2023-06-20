namespace PoundPupLegacy.EditModel.Readers;

internal sealed class ChildTraffickingCaseCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<UnresolvedChildTraffickingCase.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.CHILD_TRAFFICKING_CASE;

    private const string SQL = $"""
            {SharedSql.CASE_CREATE_CTE}
            select
                jsonb_build_object(
                    jsonb_build_object(
                        'NodeDetailsForCreate',
                        (select document from node_details_for_create_document)
                    ),
                    'Text', 
                    '',
                    'VocabularyIdTagging',
                    (select id from tagging_vocabulary),
                    'CasePartyTypesCaseParties',
                    (select document from case_type_case_party_type_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
