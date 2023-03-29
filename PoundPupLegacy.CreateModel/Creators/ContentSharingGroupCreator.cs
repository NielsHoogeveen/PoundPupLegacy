namespace PoundPupLegacy.CreateModel.Creators;

public class ContentSharingGroupCreator : IEntityCreator<ContentSharingGroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<ContentSharingGroup> contentSharingGroups, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var ownerWriter = await OwnerInserter.CreateAsync(connection);
        await using var contentSharingGroupWriter = await ContentSharingGroupInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);


        await foreach (var contentSharingGroup in contentSharingGroups) {
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
