namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartyTypeInserterFactory : SingleIdInserterFactory<CasePartyType.CasePartyTypeToCreate>
{
    protected override string TableName => "case_party_type";

    protected override bool AutoGenerateIdentity => false;

}
