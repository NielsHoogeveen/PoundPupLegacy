﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class GlobalRegionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableGlobalRegion>
{
    protected override string TableName => "global_region";

    protected override bool AutoGenerateIdentity => false;

}
