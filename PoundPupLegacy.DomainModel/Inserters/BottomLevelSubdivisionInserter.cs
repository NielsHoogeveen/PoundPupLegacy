using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class BottomLevelSubdivisionInserterFactory : SingleIdInserterFactory<BottomLevelSubdivisionToCreate>
{
    protected override string TableName => "bottom_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
