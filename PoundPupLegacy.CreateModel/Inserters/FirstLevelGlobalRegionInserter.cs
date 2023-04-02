﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstLevelGlobalRegionInserter : IDatabaseInserter<FirstLevelGlobalRegion>
{
    public static async Task<DatabaseInserter<FirstLevelGlobalRegion>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FirstLevelGlobalRegion>("first_level_global_region", connection);
    }
}
