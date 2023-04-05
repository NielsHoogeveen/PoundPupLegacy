namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BottomLevelSubdivisionInserterFactory : SingleIdInserterFactory<BottomLevelSubdivision>
{
    protected override string TableName => "bottom_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
