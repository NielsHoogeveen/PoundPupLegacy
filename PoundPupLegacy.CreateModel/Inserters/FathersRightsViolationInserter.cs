using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class FathersRightsViolationCaseInserterFactory : SingleIdInserterFactory<FathersRightsViolationCase.ToCreate>
{
    protected override string TableName => "fathers_rights_violation_case";

    protected override bool AutoGenerateIdentity => false;

}
