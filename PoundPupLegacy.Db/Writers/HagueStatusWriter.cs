﻿namespace PoundPupLegacy.Db.Writers;

internal class HagueStatusWriter : IDatabaseWriter<HagueStatus>
{
    public static async Task<DatabaseWriter<HagueStatus>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<HagueStatus>(await SingleIdWriter.CreateSingleIdCommandAsync("hague_status", connection));
    }
}
