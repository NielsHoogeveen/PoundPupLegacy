﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SystemGroupInserter : IDatabaseInserter<SystemGroup>
{
    public static async Task<DatabaseInserter<SystemGroup>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<SystemGroup>("system_group", connection);
    }
}