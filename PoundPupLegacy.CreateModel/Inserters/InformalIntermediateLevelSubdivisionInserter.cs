namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InformalIntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableInformalIntermediateLevelSubdivision>
{
    protected override string TableName => "informal_intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}