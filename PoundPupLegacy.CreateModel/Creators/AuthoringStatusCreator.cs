namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AuthoringStatusCreator : EntityCreator<AuthoringStatus>
{
    private readonly IDatabaseInserterFactory<AuthoringStatus> _authoringStatusInserterFactory;
    public AuthoringStatusCreator(IDatabaseInserterFactory<AuthoringStatus> authoringStatusInserterFactory)
    {
        _authoringStatusInserterFactory = authoringStatusInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<AuthoringStatus> authoringStatuses, IDbConnection connection)
    {

        await using var authoringStatusWriter = await _authoringStatusInserterFactory.CreateAsync(connection);

        await foreach (var authoringStatus in authoringStatuses) {
            await authoringStatusWriter.InsertAsync(authoringStatus);
        }
    }
}
