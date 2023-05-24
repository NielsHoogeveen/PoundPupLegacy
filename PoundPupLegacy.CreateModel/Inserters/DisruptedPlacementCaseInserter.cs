namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DisruptedPlacementCaseInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableDisruptedPlacementCase>
{
    protected override string TableName => "disrupted_placement_case";

    protected override bool AutoGenerateIdentity => false;

}
