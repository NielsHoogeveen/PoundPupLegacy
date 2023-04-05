namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstLevelSubdivisionInserterFactory : SingleIdInserterFactory<FirstLevelSubdivision>
{
    protected override string TableName => "first_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
