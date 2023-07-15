using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class DisruptedPlacementCaseInserterFactory : SingleIdInserterFactory<DisruptedPlacementCase.ToCreate>
{
    protected override string TableName => "disrupted_placement_case";

    protected override bool AutoGenerateIdentity => false;

}
