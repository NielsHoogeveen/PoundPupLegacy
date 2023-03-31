﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FamilySizeInserter : IDatabaseInserter<FamilySize>
{
    public static async Task<DatabaseInserter<FamilySize>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FamilySize>("family_size", connection);
    }
}