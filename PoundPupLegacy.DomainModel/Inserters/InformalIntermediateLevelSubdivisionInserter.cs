using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class InformalIntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<InformalIntermediateLevelSubdivision.ToCreate>
{
    protected override string TableName => "informal_intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}