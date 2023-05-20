namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PollStatusCreator(IDatabaseInserterFactory<PollStatus> pollStatusInserterFactory) : EntityCreator<PollStatus>
{
    public override async Task CreateAsync(IAsyncEnumerable<PollStatus> pollStatuss, IDbConnection connection)
    {
        await using var pollStatusWriter = await pollStatusInserterFactory.CreateAsync(connection);

        await foreach (var pollStatus in pollStatuss) {
            await pollStatusWriter.InsertAsync(pollStatus);
        }
    }
}
