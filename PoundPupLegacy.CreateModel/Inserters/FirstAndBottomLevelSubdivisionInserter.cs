﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstAndBottomLevelSubdivisionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableFirstAndBottomLevelSubdivision>
{
    protected override string TableName => "first_and_bottom_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}