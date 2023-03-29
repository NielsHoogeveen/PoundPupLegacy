﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CoercedAdoptionCaseInserter : IDatabaseInserter<CoercedAdoptionCase>
{
    public static async Task<DatabaseInserter<CoercedAdoptionCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CoercedAdoptionCase>("coerced_adoption_case", connection);
    }
}
