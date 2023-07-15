namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class FirstAndBottomLevelSubdivisionInserterFactory : SingleIdInserterFactory<FirstAndBottomLevelSubdivision.ToCreate>
{
    protected override string TableName => "first_and_bottom_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}