namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PublicationStatusCreator : IEntityCreator<PublicationStatus>
{
    public async Task CreateAsync(IAsyncEnumerable<PublicationStatus> publicationStatuses, IDbConnection connection)
    {

        await using var publicationStatusWriter = await PublicationStatusInserter.CreateAsync(connection);

        await foreach (var publicationStatus in publicationStatuses) {
            await publicationStatusWriter.InsertAsync(publicationStatus);
        }
    }
}
