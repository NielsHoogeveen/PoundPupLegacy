namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PollStatusCreator : EntityCreator<PollStatus>
{
    private readonly IDatabaseInserterFactory<PollStatus> _pollStatusInserterFactory;
    public PollStatusCreator(IDatabaseInserterFactory<PollStatus> pollStatusInserterFactory)
    {
        _pollStatusInserterFactory = pollStatusInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<PollStatus> pollStatuss, IDbConnection connection)
    {

        await using var pollStatusWriter = await _pollStatusInserterFactory.CreateAsync(connection);

        await foreach (var pollStatus in pollStatuss) {
            await pollStatusWriter.InsertAsync(pollStatus);
        }
    }
}
