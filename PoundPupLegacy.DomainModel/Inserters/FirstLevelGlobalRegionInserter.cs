﻿namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class FirstLevelGlobalRegionInserterFactory : SingleIdInserterFactory<FirstLevelGlobalRegion.ToCreate>
{
    protected override string TableName => "first_level_global_region";

    protected override bool AutoGenerateIdentity => false;

}