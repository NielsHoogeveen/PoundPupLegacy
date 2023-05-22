namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FormalIntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<NewFormalIntermediateLevelSubdivision>
{
    protected override string TableName => "formal_intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}