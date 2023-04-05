namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class IntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<IntermediateLevelSubdivision>
{
    protected override string TableName => "intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
