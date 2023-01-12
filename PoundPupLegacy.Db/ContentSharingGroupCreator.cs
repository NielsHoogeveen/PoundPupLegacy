namespace PoundPupLegacy.Db;

public class ContentSharingGroupCreator : IEntityCreator<ContentSharingGroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<ContentSharingGroup> contentSharingGroups, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var ownerWriter = await OwnerWriter.CreateAsync(connection);
        await using var contentSharingGroupWriter = await ContentSharingGroupWriter.CreateAsync(connection);

        await foreach (var contentSharingGroup in contentSharingGroups)
        {
            await userGroupWriter.WriteAsync(contentSharingGroup);
            await ownerWriter.WriteAsync(contentSharingGroup);
            await contentSharingGroupWriter.WriteAsync(contentSharingGroup);
        }
    }
}
