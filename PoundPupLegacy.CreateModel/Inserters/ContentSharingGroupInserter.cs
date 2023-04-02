namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ContentSharingGroupInserter : IDatabaseInserter<ContentSharingGroup>
{
    public static async Task<DatabaseInserter<ContentSharingGroup>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<ContentSharingGroup>("content_sharing_group", connection);
    }
}
