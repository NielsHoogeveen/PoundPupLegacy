namespace PoundPupLegacy.CreateModel.Creators;

public class PublicationStatusCreator : IEntityCreator<PublicationStatus>
{
    public static async Task CreateAsync(IAsyncEnumerable<PublicationStatus> publicationStatuses, NpgsqlConnection connection)
    {

        await using var publicationStatusWriter = await PublicationStatusWriter.CreateAsync(connection);

        await foreach (var publicationStatus in publicationStatuses) {
            await publicationStatusWriter.WriteAsync(publicationStatus);
        }
    }
}
