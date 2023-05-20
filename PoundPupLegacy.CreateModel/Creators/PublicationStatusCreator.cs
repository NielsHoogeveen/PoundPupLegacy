namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PublicationStatusCreator(IDatabaseInserterFactory<PublicationStatus> publicationStatusInserterFactory) : EntityCreator<PublicationStatus>
{
    public override async Task CreateAsync(IAsyncEnumerable<PublicationStatus> publicationStatuses, IDbConnection connection)
    {
        await using var publicationStatusWriter = await publicationStatusInserterFactory.CreateAsync(connection);

        await foreach (var publicationStatus in publicationStatuses) {
            await publicationStatusWriter.InsertAsync(publicationStatus);
        }
    }
}
