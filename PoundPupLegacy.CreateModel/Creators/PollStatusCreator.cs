namespace PoundPupLegacy.CreateModel.Creators;

public class PollStatusCreator : IEntityCreator<PollStatus>
{
    public async Task CreateAsync(IAsyncEnumerable<PollStatus> pollStatuss, IDbConnection connection)
    {

        await using var pollStatusWriter = await PollStatusInserter.CreateAsync(connection);

        await foreach (var pollStatus in pollStatuss) {
            await pollStatusWriter.InsertAsync(pollStatus);
        }
    }
}
