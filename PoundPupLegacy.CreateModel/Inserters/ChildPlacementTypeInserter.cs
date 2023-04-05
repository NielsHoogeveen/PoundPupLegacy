﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ChildPlacementTypeInserterFactory : SingleIdInserterFactory<ChildPlacementType>
{
    protected override string TableName => "child_placement_type";

    protected override bool AutoGenerateIdentity => false;

}
