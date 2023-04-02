﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FormalIntermediateLevelSubdivisionInserter : IDatabaseInserter<FormalIntermediateLevelSubdivision>
{
    public static async Task<DatabaseInserter<FormalIntermediateLevelSubdivision>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FormalIntermediateLevelSubdivision>("formal_intermediate_level_subdivision", connection);
    }
}