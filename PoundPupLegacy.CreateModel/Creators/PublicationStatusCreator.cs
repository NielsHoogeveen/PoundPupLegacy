namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PublicationStatusCreator : EntityCreator<PublicationStatus>
{
    private readonly IDatabaseInserterFactory<PublicationStatus> _publicationStatusInserterFactory;
    public PublicationStatusCreator(IDatabaseInserterFactory<PublicationStatus> publicationStatusInserterFactory)
    {
        _publicationStatusInserterFactory = publicationStatusInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<PublicationStatus> publicationStatuses, IDbConnection connection)
    {

        await using var publicationStatusWriter = await _publicationStatusInserterFactory.CreateAsync(connection);

        await foreach (var publicationStatus in publicationStatuses) {
            await publicationStatusWriter.InsertAsync(publicationStatus);
        }
    }
}
