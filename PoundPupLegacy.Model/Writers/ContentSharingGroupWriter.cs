namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class ContentSharingGroupWriter : IDatabaseWriter<ContentSharingGroup>
{
    public static async Task<DatabaseWriter<ContentSharingGroup>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<ContentSharingGroup>("content_sharing_group", connection);
    }
}
