﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SecondLevelSubdivisionInserter : IDatabaseInserter<SecondLevelSubdivision>
{
    public static async Task<DatabaseInserter<SecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<SecondLevelSubdivision>("second_level_subdivision", connection);
    }
}