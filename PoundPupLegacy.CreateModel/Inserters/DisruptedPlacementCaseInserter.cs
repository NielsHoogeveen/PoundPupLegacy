namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DisruptedPlacementCaseInserterFactory : SingleIdInserterFactory<DisruptedPlacementCase.DisruptedPlacementCaseToCreate>
{
    protected override string TableName => "disrupted_placement_case";

    protected override bool AutoGenerateIdentity => false;

}
