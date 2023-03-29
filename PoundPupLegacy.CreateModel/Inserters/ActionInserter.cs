namespace PoundPupLegacy.CreateModel.Inserters;
public class ActionInserter : IDatabaseInserter<Action>
{
    public static async Task<DatabaseInserter<Action>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Action>("action", connection, false);
    }
}
