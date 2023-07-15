namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class ChildPlacementTypeInserterFactory : SingleIdInserterFactory<ChildPlacementType.ToCreate>
{
    protected override string TableName => "child_placement_type";

    protected override bool AutoGenerateIdentity => false;

}
