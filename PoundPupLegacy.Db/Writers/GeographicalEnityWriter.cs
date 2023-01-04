﻿namespace PoundPupLegacy.Db.Writers;

internal class GeographicalEnityWriter : IDatabaseWriter<GeographicalEntity>
{
    public static async Task<DatabaseWriter<GeographicalEntity>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<GeographicalEntity>(await SingleIdWriter.CreateSingleIdCommandAsync("geographical_entity", connection));
    }
}
