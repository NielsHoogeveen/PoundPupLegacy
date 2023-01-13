namespace PoundPupLegacy.Db.Writers;

internal sealed class ContentSharingGroupWriter : IDatabaseWriter<ContentSharingGroup>
{
    public static async Task<DatabaseWriter<ContentSharingGroup>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<ContentSharingGroup>(await SingleIdWriter.CreateSingleIdCommandAsync("content_sharing_group", connection));
    }
}
