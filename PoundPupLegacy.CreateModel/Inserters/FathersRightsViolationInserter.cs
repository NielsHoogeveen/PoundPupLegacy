namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FathersRightsViolationCaseInserterFactory : SingleIdInserterFactory<FathersRightsViolationCase>
{
    protected override string TableName => "fathers_rights_violation_case";

    protected override bool AutoGenerateIdentity => false;

}
