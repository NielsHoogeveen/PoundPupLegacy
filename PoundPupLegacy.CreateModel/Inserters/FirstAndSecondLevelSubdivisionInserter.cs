namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstAndSecondLevelSubdivisionInserterFactory : SingleIdInserterFactory<FirstAndSecondLevelSubdivisionToCreate>
{
    protected override string TableName => "first_and_second_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}