﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class WrongfulRemovalCaseInserter : IDatabaseInserter<WrongfulRemovalCase>
{
    public static async Task<DatabaseInserter<WrongfulRemovalCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<WrongfulRemovalCase>("wrongful_removal_case", connection);
    }
}