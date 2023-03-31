﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicCountryInserter : IDatabaseInserter<BasicCountry>
{
    public static async Task<DatabaseInserter<BasicCountry>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<BasicCountry>("basic_country", connection);
    }
}