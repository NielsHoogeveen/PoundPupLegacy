namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AuthoringStatusCreator(IDatabaseInserterFactory<AuthoringStatus> authoringStatusInserterFactory) : EntityCreator<AuthoringStatus>
{
    public override async Task CreateAsync(IAsyncEnumerable<AuthoringStatus> authoringStatuses, IDbConnection connection)
    {
        await using var authoringStatusWriter = await authoringStatusInserterFactory.CreateAsync(connection);

        await foreach (var authoringStatus in authoringStatuses) {
            await authoringStatusWriter.InsertAsync(authoringStatus);
        }
    }
}
