﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstLevelSubdivisionInserter : IDatabaseInserter<FirstLevelSubdivision>
{
    public static async Task<DatabaseInserter<FirstLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FirstLevelSubdivision>("first_level_subdivision", connection);
    }
}