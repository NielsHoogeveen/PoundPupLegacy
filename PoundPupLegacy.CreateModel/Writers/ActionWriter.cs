namespace PoundPupLegacy.CreateModel.Writers;
public class ActionWriter : IDatabaseWriter<Action>
{
    public static async Task<DatabaseWriter<Action>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Action>("action", connection, false);
    }
}
