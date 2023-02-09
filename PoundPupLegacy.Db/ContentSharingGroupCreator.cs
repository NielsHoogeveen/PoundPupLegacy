namespace PoundPupLegacy.Db;

public class ContentSharingGroupCreator : IEntityCreator<ContentSharingGroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<ContentSharingGroup> contentSharingGroups, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var ownerWriter = await OwnerWriter.CreateAsync(connection);
        await using var contentSharingGroupWriter = await ContentSharingGroupWriter.CreateAsync(connection);
        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleWriter.CreateAsync(connection);


        await foreach (var contentSharingGroup in contentSharingGroups)
        {
            await userGroupWriter.WriteAsync(contentSharingGroup);
            await ownerWriter.WriteAsync(contentSharingGroup);
            await contentSharingGroupWriter.WriteAsync(contentSharingGroup);

            var administratorRole = contentSharingGroup.AdministratorRole;
            administratorRole.UserGroupId = contentSharingGroup.Id.Value;
            await principalWriter.WriteAsync(administratorRole);
            await userRoleWriter.WriteAsync(administratorRole);
            await administratorRoleWriter.WriteAsync(administratorRole);

        }
    }
}
