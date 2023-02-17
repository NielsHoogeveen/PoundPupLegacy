namespace PoundPupLegacy.Db;

public class PollStatusCreator : IEntityCreator<PollStatus>
{
    public static async Task CreateAsync(IAsyncEnumerable<PollStatus> pollStatuss, NpgsqlConnection connection)
    {

        await using var pollStatusWriter = await PollStatusWriter.CreateAsync(connection);

        await foreach (var pollStatus in pollStatuss)
        {
            await pollStatusWriter.WriteAsync(pollStatus);
        }
    }
}
