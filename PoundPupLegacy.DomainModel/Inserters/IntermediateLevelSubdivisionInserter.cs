namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class IntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<IntermediateLevelSubdivisionToCreate>
{
    protected override string TableName => "intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
