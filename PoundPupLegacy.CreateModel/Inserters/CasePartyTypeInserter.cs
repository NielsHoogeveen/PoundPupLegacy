using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CasePartyTypeInserterFactory : SingleIdInserterFactory<CasePartyType.ToCreate>
{
    protected override string TableName => "case_party_type";

    protected override bool AutoGenerateIdentity => false;

}
